using FridgeLogic.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridgeLogic.Control
{
    [RequireComponent(typeof(IMovement2D))]
    public class PlatformerInputAgent : MonoBehaviour
    {
        private IMovement2D movement = null;

        private void ProcessMovement(Vector2 inputVector)
        {
            movement.Move(inputVector);
        }

        private void ProcessJump()
        {
            // todo: hold?
            movement.Jump();
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
        }
        #endregion

        #region Unity Lifecycle
        private void Start()
        {
            movement = GetComponent<IMovement2D>();
        }
        #endregion
    }
}