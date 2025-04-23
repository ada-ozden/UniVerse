using UnityEngine;
using Mirror;
using System.Collections;

namespace Universe.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : NetworkBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _playerCamera;

        [Header("Base Movement")]
        public float runAcceleration = 50f;
        public float runSpeed = 4f;
        public float drag = 20f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private Vector2 _cameraRotation = Vector2.zero;

        [SyncVar] private Vector3 _networkPosition;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x +
                                        cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

            Vector3 movementDelta = movementDirection * runAcceleration * Time.deltaTime;
            Vector3 newVelocity = _characterController.velocity + movementDelta;

            // Apply drag
            Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(newVelocity, runSpeed);

            CmdMoveWithVelocity(newVelocity);
        }

        [Command]
        private void CmdMoveWithVelocity(Vector3 velocity)
        {
            if (_characterController == null) return;

            Vector3 movement = velocity * Time.deltaTime;
            _characterController.Move(movement);

            _networkPosition = transform.position;
            RpcUpdateMovement(_networkPosition);
        }

        [ClientRpc]
        private void RpcUpdateMovement(Vector3 newPosition)
        {
            if (!isLocalPlayer)
                StartCoroutine(SmoothMove(newPosition));
        }

        private IEnumerator SmoothMove(Vector3 targetPosition)
        {
            float elapsedTime = 0f;
            float duration = 0.1f;

            Vector3 startPosition = transform.position;
            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }

        private void LateUpdate()
        {
            if (!isLocalPlayer) return;

            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            transform.rotation = Quaternion.Euler(0f, _cameraRotation.x, 0f);
            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }
    }
}
