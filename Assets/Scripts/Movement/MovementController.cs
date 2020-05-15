using UnityEngine;

namespace FridgeLogic.Movement
{
    public class MovementController
    {
        private readonly MovementOptions _options;
        
        private float _movementX = 0;
        private float _velocityXSmoothing = 0f;
        private bool _isRunning = false;

        public MovementController(MovementOptions options)
        {
            _options = options;
        }
        
        public bool IsRunning => _isRunning;

        public void Move(Vector2 movement)
        {
            _movementX = movement.x != 0f ? Mathf.Sign(movement.x) : 0f;
        }

        public void StartRunning()
        {
            _isRunning = true;
        }

        public void StopRunning()
        {
            _isRunning = false;
        }

        public void ApplyJump(CollisionInfo collisionInfo, ref Vector2 velocity)
        {
            var runModifier = _isRunning ? _options.runSpeedModifier : 1f;
            var targetVelocityX = _movementX * _options.groundSpeed * runModifier;
            var smoothTime = _isRunning ? _options.accelerationTimeRunning : _options.accelerationTimeWalking;
            velocity.x = Mathf.SmoothDamp(
                current: velocity.x, 
                target: targetVelocityX,
                currentVelocity: ref _velocityXSmoothing,
                smoothTime: collisionInfo.below ? smoothTime : _options.accelerationTimeInAir
            );
        }
    }
}