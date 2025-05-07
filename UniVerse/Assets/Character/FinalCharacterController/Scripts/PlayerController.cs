using UnityEngine;
using Mirror;

namespace Universe.FinalCharacterController
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkTransformReliable))]     // Mirror’un pozisyon/rotasyon replike bileşeni
    [DefaultExecutionOrder(-1)]
    public class PlayerController : NetworkBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController _cc;
        [SerializeField] private Camera _playerCamera;

        [Header("Movement Settings")]
        public float runAcceleration = 50f;
        public float runSpeed        = 4f;
        public float drag            = 20f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;

        private PlayerLocomotionInput _input;
        private Vector2              _cameraRot = Vector2.zero;

        public override void OnStartLocalPlayer()
        {
            // Yerel oyuncu başladığında component referanslarını alıyoruz
            _cc    = GetComponent<CharacterController>();
            _input = GetComponent<PlayerLocomotionInput>();
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            // 1) Hareket vektörünü kamera yönüne göre oluştur
            Vector3 forward = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 right   = new Vector3(_playerCamera.transform.right.x,   0f, _playerCamera.transform.right.z).normalized;
            Vector3 dir     = right * _input.MovementInput.x + forward * _input.MovementInput.y;

            // 2) Drag ve hız sınırlandırması
            Vector3 deltaV = dir * runAcceleration * Time.deltaTime;
            Vector3 vel    = _cc.velocity + deltaV;
            Vector3 dragV  = vel.normalized * drag * Time.deltaTime;
            vel = (vel.magnitude > drag * Time.deltaTime) ? vel - dragV : Vector3.zero;
            vel = Vector3.ClampMagnitude(vel, runSpeed);

            // 3) Gerçek hareket
            _cc.Move(vel * Time.deltaTime);

            // 4) Kamera dönüşü
            _cameraRot.x += lookSenseH * _input.LookInput.x;
            _cameraRot.y = Mathf.Clamp(_cameraRot.y - lookSenseV * _input.LookInput.y, -lookLimitV, lookLimitV);

            transform.rotation               = Quaternion.Euler(0f, _cameraRot.x, 0f);
            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRot.y, _cameraRot.x, 0f);
        }
    }
}