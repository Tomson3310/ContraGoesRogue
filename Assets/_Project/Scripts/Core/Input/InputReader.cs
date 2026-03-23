using UnityEngine;

namespace ContraGoesRogue.Core.Input
{
    /// <summary>
    /// Handles player input and exposes it as simple values.
    /// </summary>
    public class InputReader : MonoBehaviour
    {
        private InputSystem_Actions inputActions;

        public Vector2 MoveInput { get; private set; }
        public bool JumpTriggered { get; private set; }

        private void Awake()
        {
            inputActions = new InputSystem_Actions();

            inputActions.Player.Move.performed += context => MoveInput = context.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += context => MoveInput = Vector2.zero;

            inputActions.Player.Jump.performed += context => JumpTriggered = true;
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        // Resets jump trigger after it has been processed
        public void ConsumeJump()
        {
            JumpTriggered = false;
        }
    }
}