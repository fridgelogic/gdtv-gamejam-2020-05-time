using FridgeLogic.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridgeLogic.Control
{
    [RequireComponent(typeof(PlatformerController))]
    public class PlatformerInputAgent : MonoBehaviour
    {
        private PlatformerController _platformerController = null;
        private PlatformerController PlatformerController => _platformerController ?? (_platformerController = GetComponent<PlatformerController>());

        #region Input Events
        public void OnMove(InputAction.CallbackContext context)
        {
            var movement = context.ReadValue<Vector2>();
            if (Mathf.Abs(movement.x) < 0.1f)
            {
                movement.x = 0f;
            }
            PlatformerController.Move(movement);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    PlatformerController.StartJumping();
                    break;
                case InputActionPhase.Canceled:
                    PlatformerController.StopJumping();
                    break;
                default: break;
            }
        }

        public void OnHoldShiftMoveSpeed(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    PlatformerController.StartRunning();
                    break;
                case InputActionPhase.Canceled:
                    PlatformerController.StopRunning();
                    break;
                default: break;
            }
        }
        #endregion
    }
}