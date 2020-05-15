using UnityEngine;

namespace FridgeLogic.Movement
{
    public class JumpController
    {
        private readonly MovementOptions options;
        
        private float _gravity = 0f;
        private float _jumpVelocity = 0f;
        private bool _isJumping = false;
        private bool _stopJump = false;
        private float _jumpSentAt = float.MinValue;
        private float _leftGroundAt = float.MinValue;
        private bool _groundedLastFrame = false;

        public float Gravity => _gravity;
        public bool IsJumping => _isJumping;

        public JumpController(MovementOptions options)
        {
            this.options = options;
            RecalculateJumpParameters();
        }

        public void StartJumping() => _jumpSentAt = Time.time;

        public void StopJumping()
        {
            _stopJump = true;
            _jumpSentAt = float.MinValue;
        }

        public void ApplyMovement(CollisionInfo collisionInfo, ref Vector2 velocity)
        {
            if (collisionInfo.above || collisionInfo.below)
            {
                velocity.y = 0;
            }

            var grounded = collisionInfo.below;
            var shouldJump = _jumpSentAt + options.jumpBufferTime > Time.time;
            var canJump = grounded || _leftGroundAt + options.coyoteTime > Time.time;
            canJump = canJump & !_isJumping;

            if (shouldJump && canJump)
            {
                _jumpSentAt = float.MinValue;
                velocity.y += _jumpVelocity;
                _isJumping = true;
            }
            else if (_stopJump)
            {
                _stopJump = false;
                if (velocity.y > 0f)
                {
                    velocity.y *= options.stopJumpRate;
                }
            }

            if (_groundedLastFrame && !grounded)
            {
                _leftGroundAt = Time.time;
            }

            if (_isJumping && velocity.y <= 0f)
            {
                _isJumping = false;
            }
            
            _groundedLastFrame = grounded;
        }

        // TODO: observable options collection and recalculate on change?
        public void RecalculateJumpParameters()
        {
            _gravity = -(2 * options.jumpHeight) / Mathf.Pow(options.timeToJumpApex, 2);
            _jumpVelocity = Mathf.Abs(_gravity * options.timeToJumpApex);
        }
    }
}