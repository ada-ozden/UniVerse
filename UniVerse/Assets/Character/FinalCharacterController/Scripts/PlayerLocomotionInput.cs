using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

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
        public override void OnStartLocalPlayer()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }
      
        private void OnDisable()
        {
            if (!isLocalPlayer || PlayerControls == null) return;

            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }


        public void OnMovement(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer || IsTyping) return;

            MovementInput = context.ReadValue<Vector2>();
        }
        

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;

            LookInput = context.ReadValue<Vector2>();
        }
    }
}
