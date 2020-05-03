using System;
using FridgeLogic.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FridgeLogic.Control
{
    [RequireComponent(typeof(IMovement2D))]
    public class PlatformerInputAgent : MonoBehaviour
    {
        private PlatformerInputMap inputMap = null;
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
        private void OnMove(InputAction.CallbackContext context)
        {
            ProcessMovement(context.ReadValue<Vector2>());
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            ProcessJump();
        }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            
            inputMap = new PlatformerInputMap();
            inputMap.Gameplay.Move.performed += OnMove;
            inputMap.Gameplay.Jump.performed += OnJump;
        }

        private void Start()
        {
            movement = GetComponent<IMovement2D>();
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
            
        }
        #endregion
    }
}