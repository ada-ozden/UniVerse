using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

namespace Universe.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : NetworkBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        // Prevents movement when typing
        public bool IsTyping { get; set; }

        private void OnEnable()
        {
            if (!IsOwner) return; // Only the owning client should read input.

            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (!IsOwner) return;

            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (!IsOwner || IsTyping) return; // Only process if not typing

            MovementInput = context.ReadValue<Vector2>();
            SubmitMovementServerRpc(MovementInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            LookInput = context.ReadValue<Vector2>();
        }

        [ServerRpc]
        private void SubmitMovementServerRpc(Vector2 movementInput)
        {
            MovementInput = movementInput; // Sync movement input with server
        }
        
    }
}
