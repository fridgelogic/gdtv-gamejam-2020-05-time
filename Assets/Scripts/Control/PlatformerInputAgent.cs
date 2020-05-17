using FridgeLogic.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridgeLogic.Control
{
    [RequireComponent(typeof(IMovement2D))]
    public class PlatformerInputAgent : MonoBehaviour
    {
        private IMovement2D _movement = null;
        private IMovement2D Movement
        {
            get
            {
                if (_movement == null)
                {
                    _movement = GetComponent<IMovement2D>();
                }
                return _movement;
            }
        }

        private void ProcessMovement(Vector2 inputVector)
        {
            Movement.Move(inputVector);
        }

        private void ProcessJump()
        {
            Movement.StartJump();
        }

        private void CancelJump()
        {
            Movement.StopJump();
        }

        private void StartRun()
        {
            Movement.StartRunning();
        }

        private void StopRun()
        {
            Movement.StopRunning();
        }

        #region Input Events
        public void OnMove(InputAction.CallbackContext context)
        {
            ProcessMovement(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ProcessJump();
            }
            else if (context.canceled)
            {
                CancelJump();
            }
        }

        public void OnHoldShiftMoveSpeed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                StartRun();
            }
            else if (context.canceled)
            {
                StopRun();
            }
        }
        #endregion
    }
}