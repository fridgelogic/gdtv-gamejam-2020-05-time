using System;
using System.Collections.Generic;
using UnityEngine;

/*
    Based on the work of Sebastian Lague
    2D Platformer Series: https://www.youtube.com/playlist?list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz
*/

namespace FridgeLogic.Movement
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlatformerCollider2d : MonoBehaviour
    {
        #region Inspector Fields
            [SerializeField]
            [Min(0)]
            private int assetsPixelsPerUnit = 100;

            [SerializeField]
            [Range(2, 64)]
            private int horizontalRayCount = 4;

            [SerializeField]
            [Range(2, 64)]
            private int verticalRayCount = 4;

            [SerializeField]
            [Range(0, 90)]
            private float maxClimbingAngle = 60f;

            [SerializeField]
            [Range(0, 90)]
            private float maxDescendingAngle = 60f;

            [SerializeField]
            private LayerMask layerMask = default(LayerMask);
        #endregion

        // Public Properties
        public CollisionInfo CollisionInfo => collisionInfo;

        // Component References
        private BoxCollider2D boxCollider = null;

        // Calculated Values
        private RaycastOrigins raycastOrigins;
        private CollisionInfo collisionInfo;
        private float colliderInset;
        private float horizontalRaySpacing;
        private float verticalRaySpacing;
        private Vector2 unmodifiedVelocity;

        public Vector2 UpdateCollisions(Vector2 velocity)
        {
            UpdateRaycastOrigins();
            collisionInfo.Reset();
            unmodifiedVelocity = velocity;

            if (velocity.y < 0)
            {
                velocity = CalculateSlopeDescentVelocity(velocity);
            }
            if (velocity.x != 0)
            {
                velocity = CalculateHorizontalCollisions(velocity);
            }
            if (velocity.y != 0)
            {
                velocity = CalculateVerticalCollisions(velocity);
            }
            return velocity;
        }

        private Vector2 CalculateHorizontalCollisions(Vector2 velocity)
        {
            var direction = Mathf.Sign(velocity.x);
            var rayLength = Mathf.Abs(velocity.x) + colliderInset;
            var raysInfo = (direction < 0) ? CalculateRaysLeft(rayLength) : CalculateRaysRight(rayLength);
            var checkSlope = true;

            foreach (var rayInfo in raysInfo)
            {
                if (rayInfo.hit)
                {
                    var slope = Vector2.Angle(rayInfo.hit.normal, Vector2.up);

                    // Determine whether or not we're climbing a slope.
                    // We're only checking on the lowest x ray.
                    if (checkSlope && slope <= maxClimbingAngle)
                    {
                        // Check to see if we are currently descending a slope and approaching a climb
                        if (collisionInfo.descendingSlope)
                        {
                            // If so, reset the descending slope flag and the velocity
                            collisionInfo.descendingSlope = false;
                            velocity = unmodifiedVelocity;
                        }

                        // Check to see how close we are to being on the slope
                        var distanceToSlope = 0f;
                        if (slope != collisionInfo.prevSlope)
                        {
                            // We're still moving toward the slope,
                            // make sure we move onto it fully before starting
                            // to climb
                            distanceToSlope = rayInfo.hit.distance - colliderInset;
                            velocity.x -= distanceToSlope * direction;
                        }

                        var climbVelocity = CalculateSlopeClimbVelocity(velocity, slope);
                        // Check to see if we're climbing faster than we're moving upwards now
                        // (just in case we're jumping)
                        if (climbVelocity.y > velocity.y)
                        {
                            velocity.x = climbVelocity.x;
                            velocity.y = climbVelocity.y;
                            collisionInfo.below = true;
                            collisionInfo.climbingSlope = true;
                            collisionInfo.slope = slope;
                        }

                        // If we were moving onto the slope, step forward
                        velocity.x += distanceToSlope * direction;
                    }

                    if (!collisionInfo.climbingSlope || slope > maxClimbingAngle)
                    {
                        // We're not climbing a slope
                        velocity.x = (rayInfo.hit.distance - colliderInset) * direction;

                        // If we detect an unclimbable slope to the side while
                        // climbing a slope, make sure the y velocity reflects that
                        // we will not be moving up
                        if (collisionInfo.climbingSlope)
                        {
                            velocity.y = Mathf.Tan(collisionInfo.slope * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                        }

                        collisionInfo.left = direction < 0;
                        collisionInfo.right = direction > 0;
                    }
                }

                // Only check it on the first call
                checkSlope = false;
            }

            return velocity;
        }

        private Vector2 CalculateVerticalCollisions(Vector2 velocity)
        {
            var direction = Mathf.Sign(velocity.y);
            var rayLength = Mathf.Abs(velocity.y) + colliderInset;
            var originAdjustment = new Vector2(velocity.x, 0);
            var raysInfo = (direction < 0) ? CalculateRaysDown(rayLength, originAdjustment) : CalculateRaysUp(rayLength, originAdjustment);
            foreach (var rayInfo in raysInfo)
            {
                if (rayInfo.hit)
                {
                    Debug.DrawRay(rayInfo.ray.origin, Vector2.up * direction * rayInfo.hit.distance, Color.red);

                    velocity.y = (rayInfo.hit.distance - colliderInset) * direction;

                    if (collisionInfo.climbingSlope)
                    {
                        velocity.x = velocity.y / Mathf.Tan(collisionInfo.slope * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    }

                    collisionInfo.below = direction < 0;
                    collisionInfo.above = direction > 0;
                }
            }

            // Check to see if we are going from one slope angle to a different angle
            if (collisionInfo.climbingSlope)
            {
                // Fire a horizontal ray in the direction we're moving
                var directionX = Mathf.Sign(velocity.x);
                rayLength = Mathf.Abs(velocity.x) + colliderInset;
                var origin = (directionX < 0) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                // Cast from our next height
                origin += Vector2.up * velocity.y;
                var hit = Physics2D.Raycast(origin, Vector2.right * directionX, rayLength, layerMask);

                if (hit)
                {
                    var slope = Vector2.Angle(hit.normal, Vector2.up);
                    if (slope != collisionInfo.slope)
                    {
                        // We are about to step onto a different angled slope
                        var distanceToSlope = hit.distance - colliderInset;
                        velocity.x = distanceToSlope * directionX;
                        collisionInfo.slope = slope;
                    }
                }
            }

            return velocity;
        }

        /// <summary>
        /// Calculates the velocity needed to maintain the same horizontal
        /// movement speed on a slope.
        /// </summary>
        /// <param name="velocity">The current velocity</param>
        /// <param name="slope">The angle of the slope</param>
        /// <returns></returns>
        private Vector2 CalculateSlopeClimbVelocity(Vector2 velocity, float slope)
        {
            var moveDistance = Mathf.Abs(velocity.x);
            return new Vector2(
                // Trig: Length = cos(angle) * Hypotenuse
                x: Mathf.Cos(slope * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x),
                // Trig: Height = sin(angle) * Hypotenuse
                y: Mathf.Sin(slope * Mathf.Deg2Rad) * moveDistance
            );
        }

        private Vector2 CalculateSlopeDescentVelocity(Vector2 velocity)
        {
            var direction = Mathf.Sign(velocity.x);
            var rayOrigin = (direction < 0) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            var hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, layerMask);

            if (hit)
            {
                var slope = Vector2.Angle(hit.normal, Vector2.up);
                if (slope != 0 && slope <= maxDescendingAngle)
                {
                    // Check to see if it is sloping in the direct we are heading
                    if (Mathf.Sign(hit.normal.x) == direction)
                    {
                        // Check how far we are from the slope
                        var distanceToSlope = Mathf.Tan(slope * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                        if ((hit.distance - colliderInset) <= distanceToSlope)
                        {
                            // Trig: Hypotenuse
                            var moveDistance = Mathf.Abs(velocity.x);
                            // Trig: Triangle height
                            var descentVelocity = Mathf.Sin(slope * Mathf.Deg2Rad) * moveDistance;
                            // Trig: Triangle length
                            velocity.x = Mathf.Cos(slope * Mathf.Deg2Rad) * moveDistance * direction;
                            velocity.y -= descentVelocity;

                            collisionInfo.slope = slope;
                            collisionInfo.descendingSlope = true;
                            collisionInfo.below = true;
                        }
                    }
                }
            }

            return velocity;
        }

        private void CalculateColliderInset()
        {
            // Inset by 1 pixel
            colliderInset = 1f / assetsPixelsPerUnit;
        }

        private void UpdateRaycastOrigins()
        {
            var bounds = boxCollider.bounds;
            // Apply the collider inset
            bounds.Expand(colliderInset * -2);
            raycastOrigins = new RaycastOrigins(bounds);
        }

        private void CalculateRaySpacing()
        {
            var bounds = boxCollider.bounds;
            // Apply the collider inset
            bounds.Expand(colliderInset * -2);
            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        private IEnumerable<RayInfo2D> CalculateRaysUp(float maxRayLength = 1f, Vector2? originAdjustment = null)
        {
            var adjust = originAdjustment ?? Vector2.zero;
            return CalculateRays(verticalRayCount, raycastOrigins.topLeft + adjust, Vector2.right, verticalRaySpacing, Vector2.up, layerMask, maxRayLength);
        }

        private IEnumerable<RayInfo2D> CalculateRaysDown(float maxRayLength = 1f, Vector2? originAdjustment = null)
        {
            var adjust = originAdjustment ?? Vector2.zero;
            return CalculateRays(verticalRayCount, raycastOrigins.bottomLeft + adjust, Vector2.right, verticalRaySpacing, Vector2.down, layerMask, maxRayLength);
        }

        private IEnumerable<RayInfo2D> CalculateRaysLeft(float maxRayLength = 1f, Vector2? originAdjustment = null)
        {
            var adjust = originAdjustment ?? Vector2.zero;
            return CalculateRays(horizontalRayCount, raycastOrigins.bottomLeft + adjust, Vector2.up, horizontalRaySpacing, Vector2.left, layerMask, maxRayLength);
        }

        private IEnumerable<RayInfo2D> CalculateRaysRight(float maxRayLength = 1f, Vector2? originAdjustment = null)
        {
            var adjust = originAdjustment ?? Vector2.zero;
            return CalculateRays(horizontalRayCount, raycastOrigins.bottomRight + adjust, Vector2.up, horizontalRaySpacing, Vector2.right, layerMask, maxRayLength);
        }

        private static IEnumerable<RayInfo2D> CalculateRays(
            int rayCount,
            Vector2 origin,
            Vector2 spacingDirection,
            float spacing,
            Vector2 direction,
            LayerMask layerMask,
            float maxRayLength)
        {
            var rayLength = maxRayLength;

            for (int i = 0; i < rayCount; i++)
            {
                var from = origin + spacingDirection * spacing * i;
                var hit = Physics2D.Raycast(
                    origin: from,
                    direction: direction,
                    distance: rayLength,
                    layerMask: layerMask
                );

                if (hit)
                {
                    rayLength = hit.distance;
                }

                yield return new RayInfo2D(
                    origin: from,
                    direction: direction,
                    hit: hit
                );
            }
        }

        #region Unity Lifecycle
        private void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            CalculateColliderInset();
            CalculateRaySpacing();
        }
        #endregion

        #region Editor
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                boxCollider = GetComponent<BoxCollider2D>();
                CalculateColliderInset();
                UpdateRaycastOrigins();
                CalculateRaySpacing();
            }

            foreach (var rayInfo in CalculateRaysDown())
            {
                if (rayInfo.hit)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction * rayInfo.hit.distance);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction);
                }
            }

            foreach (var rayInfo in CalculateRaysUp())
            {
                if (rayInfo.hit)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction * rayInfo.hit.distance);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction);
                }
            }

            foreach (var rayInfo in CalculateRaysLeft())
            {
                if (rayInfo.hit)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction * rayInfo.hit.distance);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction);
                }
            }

            foreach (var rayInfo in CalculateRaysRight())
            {
                if (rayInfo.hit)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction * rayInfo.hit.distance);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction);
                }
            }
        }
        #endregion

        #region RaycastOrigins
        private struct RaycastOrigins
        {
            public Vector2 topLeft;
            public Vector2 topRight;
            public Vector2 bottomLeft;
            public Vector2 bottomRight;

            public RaycastOrigins(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
            {
                this.topLeft = topLeft;
                this.topRight = topRight;
                this.bottomLeft = bottomLeft;
                this.bottomRight = bottomRight;
            }

            public RaycastOrigins(Bounds bounds)
            {
                topLeft = new Vector2(bounds.min.x, bounds.max.y);
                topRight = bounds.max;
                bottomLeft = bounds.min;
                bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            }

            public void SetToBounds(Bounds bounds)
            {
                topLeft = new Vector2(bounds.min.x, bounds.max.y);
                topRight = bounds.max;
                bottomLeft = bounds.min;
                bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            }
        }
        #endregion

        #region RayInfo2D
        private struct RayInfo2D
        {
            public Ray2D ray;
            public RaycastHit2D hit;

            public RayInfo2D(Vector2 origin, Vector2 direction, RaycastHit2D hit)
            {
                ray = new Ray2D(origin, direction);
                this.hit = hit;
            }
        }
        #endregion
    }

    #region CollisionInfo
    public struct CollisionInfo
    {
        public bool above;
        public bool below;
        public bool left;
        public bool right;
        public bool climbingSlope;
        internal bool descendingSlope;
        public float slope;
        public float prevSlope;

        public void Reset()
        {
            above = false;
            below = false;
            left = false;
            right = false;
            climbingSlope = false;
            descendingSlope = false;
            prevSlope = slope;
            slope = 0;
        }
    }
    #endregion
}