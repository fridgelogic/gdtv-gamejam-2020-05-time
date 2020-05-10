using System.Collections.Generic;
using UnityEngine;

/*
    Based on the work of Sebastian Lague
    2D Platformer Series: https://www.youtube.com/playlist?list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz
*/

namespace FridgeLogic.Movement
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaycastCollider2D : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField]
        [Min(0)]
        protected int assetsPixelsPerUnit = 100;

        [SerializeField]
        [Range(2, 64)]
        protected int horizontalRayCount = 4;

        [SerializeField]
        [Range(2, 64)]
        protected int verticalRayCount = 4;

        [SerializeField]
        protected LayerMask layerMask = default(LayerMask);
        #endregion

        // Protected accessors
        protected float ColliderInset => colliderInset;
        protected RaycastOriginCollection RaycastOrigins => raycastOrigins;

        // Component References
        private BoxCollider2D boxCollider = null;

        private BoxCollider2D BoxCollider
        {
            get
            {
                if (!boxCollider)
                {
                    boxCollider = GetComponent<BoxCollider2D>();
                }

                return boxCollider;
            }
        }

        // Calculated Values
        private RaycastOriginCollection raycastOrigins;
        private float colliderInset;
        private float horizontalRaySpacing;
        private float verticalRaySpacing;

        protected void UpdateRaycastOrigins()
        {
            var bounds = BoxCollider.bounds;
            // Apply the collider inset
            bounds.Expand(colliderInset * -2);
            raycastOrigins = new RaycastOriginCollection(bounds);
        }

        private void CalculateColliderInset()
        {
            // Inset by 1 pixel
            colliderInset = 1f / assetsPixelsPerUnit;
        }

        private void CalculateRaySpacing()
        {
            var bounds = BoxCollider.bounds;
            // Apply the collider inset
            bounds.Expand(colliderInset * -2);
            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        protected IEnumerable<RayInfo2D> CalculateRaysUp(float maxRayLength = 1f, Vector2? originAdjustment = null)
        {
            var adjust = originAdjustment ?? Vector2.zero;
            return CalculateRays(verticalRayCount, raycastOrigins.topLeft + adjust, Vector2.right, verticalRaySpacing, Vector2.up, layerMask, maxRayLength);
        }

        protected IEnumerable<RayInfo2D> CalculateRaysDown(float maxRayLength = 1f, Vector2? originAdjustment = null)
        {
            var adjust = originAdjustment ?? Vector2.zero;
            return CalculateRays(verticalRayCount, raycastOrigins.bottomLeft + adjust, Vector2.right, verticalRaySpacing, Vector2.down, layerMask, maxRayLength);
        }

        protected IEnumerable<RayInfo2D> CalculateRaysLeft(float maxRayLength = 1f, Vector2? originAdjustment = null)
        {
            var adjust = originAdjustment ?? Vector2.zero;
            return CalculateRays(horizontalRayCount, raycastOrigins.bottomLeft + adjust, Vector2.up, horizontalRaySpacing, Vector2.left, layerMask, maxRayLength);
        }

        protected IEnumerable<RayInfo2D> CalculateRaysRight(float maxRayLength = 1f, Vector2? originAdjustment = null)
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
        protected virtual void Start()
        {
            CalculateColliderInset();
            CalculateRaySpacing();
        }
        #endregion

        #region Editor
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
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
        protected struct RaycastOriginCollection
        {
            public Vector2 topLeft;
            public Vector2 topRight;
            public Vector2 bottomLeft;
            public Vector2 bottomRight;

            public RaycastOriginCollection(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
            {
                this.topLeft = topLeft;
                this.topRight = topRight;
                this.bottomLeft = bottomLeft;
                this.bottomRight = bottomRight;
            }

            public RaycastOriginCollection(Bounds bounds)
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
        protected struct RayInfo2D
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
}