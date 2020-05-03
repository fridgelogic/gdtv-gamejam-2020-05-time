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
            [Min(0)]
            private float jumpHeight = 4f;

            [SerializeField]
            [Min(0)]
            private float timeToJumpApex = 0.4f;

            [SerializeField]
            [Min(0)]
            private float accelerationTimeInAir = 0.2f;
            
            [SerializeField]
            [Min(0)]
            private float accelerationTimeOnGround = 0.1f;
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
        private bool jumpPressed = false;

        public void Move(Vector2 movement)
        {
            this.movement = movement;
        }

        public void Jump()
        {
            jumpPressed = true;
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

            if (jumpPressed)
            {
                if (platformerCollider.CollisionInfo.below)
                {
                    velocity.y += jumpVelocity;
                }

                jumpPressed = false;
            }
        }

        private void HandleMovement()
        {
            var targetVelocityX = movement.x * groundSpeed;
            velocity.x = Mathf.SmoothDamp(
                current: velocity.x, 
                target: targetVelocityX,
                currentVelocity: ref velocityXSmoothing,
                smoothTime: platformerCollider.CollisionInfo.below ? accelerationTimeOnGround : accelerationTimeInAir
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
        // TODO These are moving


        private void UpdateHorizontalFacing(float horizontalFacing)
        {
            GetComponent<Animator>().SetFloat("FacingX", horizontalFacing);
        }

        private void UpdateHorizontalSpeed(float horizontalSpeed)
        {
            GetComponent<Animator>().SetFloat("HorizontalSpeed", horizontalSpeed);
        }
        #endregion

        #region Unity Lifecycle
            private void Awake()
            {
                CalculateJumpParameters();
            }

            private void Start()
            {
                platformerCollider = GetComponent<PlatformerCollider2d>();
            }

            private void FixedUpdate()
            {
                ResetVelocity();
                HandleJump();
                HandleMovement();
                var translation = CalculateTranslation();
                CurrentMovement = platformerCollider.UpdateCollisions(translation);
                transform.Translate(CurrentMovement);

                UpdateHorizontalFacing(Mathf.Sign(velocity.x));
                UpdateHorizontalSpeed(Mathf.Abs(CurrentMovement.x) > 0 ? Mathf.Abs(velocity.x) : 0f);
            }
        #endregion
    }

    public interface IMovement2D
    {
        void Move(Vector2 movement);
        void Jump();
    }
}