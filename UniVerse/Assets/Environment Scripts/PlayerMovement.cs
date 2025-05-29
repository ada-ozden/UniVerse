using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Hareket hızı
    public float rotationSpeed = 700f; // Dönüş hızı
    public Transform cameraTransform; // Kameranın Transform'u
    public float gravity = -9.8f; // Yerçekimi
    public float jumpHeight = 2f; // Zıplama yüksekliği

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Zeminde olup olmadığını kontrol et
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Yüzeye hafifçe yapışmasını sağla
        }

        // Klavye girdiğini al
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Kameranın yönüne göre hareketi belirle
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Kameraya göre hedef açıyı hesapla
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);

            // Objeyi döndür
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Hareket yönü
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            // Hareketi uygula
            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        // Zıplama
        if (isGrounded && Input.GetButtonDown("Jump")) // Varsayılan Jump tuşu: Space
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Zıplama hızını hesapla
        }

        // Yerçekimini uygula
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
