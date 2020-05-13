using FridgeLogic.ScriptableObjects.GameEvents;
using UnityEngine;

namespace FridgeLogic.Movement
{
    [RequireComponent(typeof(PlatformerCollider2d))]
    public class PlatformerMovement : MonoBehaviour, IMovement2D
    {
        #region Inspector Fields
        [SerializeField]
        private float groundSpeed = 12f;

        [SerializeField]
        private float runSpeedModifier = 1.75f;

        [SerializeField]
        [Min(0)]
        private float jumpHeight = 4f;

        [SerializeField]
        [Min(0)]
        private float timeToJumpApex = 0.4f;

        [SerializeField]
        [Min(0)]
        private float accelerationTimeInAir = 0.25f;
        
        [SerializeField]
        [Min(0)]
        private float accelerationTimeWalking = 0.1f;

        [SerializeField]
        [Min(0)]
        private float accelerationTimeRunning = 0.2f;

        [SerializeField]
        private float jumpBufferTime = 0.1f;

        [SerializeField]
        private float coyoteTime = 0.1f;

        [SerializeField]
        [Range(0, 0.8f)]
        private float stopJumpRate = 0.5f;

        [SerializeField]
        private GameEvent jumped = null;
        #endregion

        public Vector2 CurrentMovement { get; private set; }

        // Component References
        private PlatformerCollider2d platformerCollider;

        // Calculated Values
        private Vector2 movement = Vector2.zero;
        private Vector2 velocity = Vector2.zero;
        private float gravity = 0f;
        private float jumpVelocity = 0f;
        private float velocityXSmoothing = 0f;
        private float jumpSentAt = float.MinValue;
        private float leftGroundAt = float.MinValue;
        private bool groundedLastFrame = false;
        private bool stopJump = false;
        private bool isRunning = false;
        private bool isJumping = false;

        public void Move(Vector2 movement)
        {
            this.movement = movement;
        }

        public void StartJump()
        {
            jumpSentAt = Time.time;
        }

        public void StopJump()
        {
            stopJump = true;
            jumpSentAt = float.MinValue;
        }

        public void StartRunning()
        {
            isRunning = true;
        }

        public void StopRunning()
        {
            isRunning = false;
        }

        private void CalculateJumpParameters()
        {
            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        }

        private void ResetVelocity()
        {
            if (platformerCollider.CollisionInfo.above || platformerCollider.CollisionInfo.below)
            {
                velocity.y = 0;
            }
        }

        private void HandleJump()
        {
            if (Application.isEditor)
            {
                CalculateJumpParameters();
            }

            var grounded = platformerCollider.CollisionInfo.below;
            var shouldJump = jumpSentAt + jumpBufferTime > Time.time;
            var canJump = grounded || leftGroundAt + coyoteTime > Time.time;
            canJump = canJump & !isJumping;

            if (shouldJump && canJump)
            {
                jumpSentAt = float.MinValue;
                velocity.y += jumpVelocity;
                isJumping = true;
                jumped?.Raise();
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0f)
                {
                    velocity.y *= stopJumpRate;
                }
            }

            if (groundedLastFrame && !grounded)
            {
                leftGroundAt = Time.time;
            }

            if (isJumping && velocity.y <= 0f)
            {
                isJumping = false;
            }
            
            groundedLastFrame = grounded;
        }

        private void HandleMovement()
        {
            var runModifier = isRunning ? runSpeedModifier : 1f;
            var targetVelocityX = movement.x * groundSpeed * runModifier;
            var smoothTime = isRunning ? accelerationTimeRunning : accelerationTimeWalking;
            velocity.x = Mathf.SmoothDamp(
                current: velocity.x, 
                target: targetVelocityX,
                currentVelocity: ref velocityXSmoothing,
                smoothTime: platformerCollider.CollisionInfo.below ? smoothTime : accelerationTimeInAir
            );
        }

        // https://en.wikipedia.org/wiki/Verlet_integration#Velocity_Verlet
        private Vector2 CalculateTranslation()
        {
            var position = new Vector2(transform.position.x, transform.position.y);
            var acceleration = new Vector2(0, gravity);
            var translation = velocity * Time.deltaTime + acceleration * Mathf.Pow(Time.deltaTime, 2) * 0.5f;
            velocity += acceleration * Time.deltaTime;
            return translation;
        }

        #region Animations
        // TODO Move these somewhere better
        private void UpdateAnimator()
        {
            var animator = GetComponent<Animator>();
            if (platformerCollider.CollisionInfo.below || groundedLastFrame)
            {
                if (Mathf.Abs(CurrentMovement.x) >= 0.001)
                {
                    var moveRate = Mathf.Abs(velocity.x) / (runSpeedModifier * groundSpeed);
                    animator.SetFloat("FacingX", Mathf.Sign(CurrentMovement.x));
                    animator.SetFloat("HorizontalSpeed", moveRate * 2);
                    animator.SetBool("IsRunning", Mathf.Abs(velocity.x) > groundSpeed);
                }
                else
                {
                    animator.SetFloat("HorizontalSpeed", 0);
                    animator.SetBool("IsRunning", false);
                }
            }

            animator.SetBool("IsGrounded", platformerCollider.CollisionInfo.below);
            //animator.SetFloat("VelocityY", velocity.y);
        }
        #endregion

        #region Unity Lifecycle
            private void Start()
            {
                platformerCollider = GetComponent<PlatformerCollider2d>();
                CalculateJumpParameters();
            }

            private void Update()
            {
                ResetVelocity();
                HandleJump();
                HandleMovement();

                var translation = CalculateTranslation();
                CurrentMovement = platformerCollider.UpdateCollisions(translation);
                transform.Translate(CurrentMovement);

                UpdateAnimator();
            }
        #endregion
    }

    public interface IMovement2D
    {
        void Move(Vector2 movement);
        void StartJump();
        void StopJump();
        void StartRunning();
        void StopRunning();
    }
}