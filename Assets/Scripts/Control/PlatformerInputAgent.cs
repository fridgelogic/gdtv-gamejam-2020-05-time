using UnityEngine;
using UnityEngine.InputSystem;

namespace FridgeLogic.Control
{
    public class PlatformerInputAgent : MonoBehaviour
    {
        private PlatformerInputMap inputMap = null;
        private Vector2 moveVector = Vector2.zero;

        private void ProcessMovement(Vector2 inputVector)
        {
            moveVector = inputVector;
        }

        #region Input Events
        private void OnMove(InputAction.CallbackContext context)
        {
            ProcessMovement(context.ReadValue<Vector2>());
        }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            
            inputMap = new PlatformerInputMap();
            inputMap.Gameplay.Move.performed += OnMove;
        }

        private void OnEnable()
        {
            inputMap.Enable();
        }

        private void OnDisable()
        {
            inputMap.Disable();
        }

        private void Update()
        {
            if (moveVector != Vector2.zero)
            {
                Debug.Log("Moving " + moveVector);
            }
        }
        #endregion
    }
}