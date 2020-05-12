using FridgeLogic.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridgeLogic.Control
{
    [RequireComponent(typeof(IMovement2D))]
    public class PlatformerInputAgent : MonoBehaviour
    {
        

        public IMovement2D _movement = null;
        private IMovement2D Movement => _movement ?? (_movement = GetComponent<IMovement2D>());

        private void ProcessMovement(Vector2 inputVector)
        {
            Movement.Move(inputVector);
        }

        private void ProcessJump() => Movement.StartJump();

        private void CancelJump() => Movement.StopJump();

        private void StartRun() => Movement.StartRunning();

        private void StopRun() => Movement.StopRunning();

        #region Input Events
        public void OnMove(InputAction.CallbackContext context) => ProcessMovement(context.ReadValue<Vector2>());

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    ProcessJump();
                    break;
                case InputActionPhase.Canceled:
                    CancelJump();
                    break;
                default: break;
            }
        }

        public void OnHoldShiftMoveSpeed(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    StartRun();
                    break;
                case InputActionPhase.Canceled:
                    StopRun();
                    break;
                default: break;
            }
        }
        #endregion
    }
}