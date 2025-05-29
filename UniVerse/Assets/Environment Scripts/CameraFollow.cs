using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Takip edilecek oyuncu
    public Vector3 offset;   // Kamera ile hedef arasındaki mesafe
    public float rotationSpeed = 3f; // Kamera döndürme hızı
    public bool invertY = false;     // Y eksenini ters çevirmek ister misiniz?

    private float currentRotationX = 0f; // Kameranın X rotasyonu
    private float currentRotationY = 0f; // Kameranın Y rotasyonu

    void LateUpdate()
    {
        // Fare sağ tuşa basılıyken kamerayı döndür
        if (Input.GetMouseButton(1)) // Sağ fare tuşu
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            currentRotationX += invertY ? mouseY : -mouseY; // Y ekseni hareketi
            currentRotationY += mouseX; // X ekseni hareketi

            // X eksenini sınırla (örneğin: dik bakışı sınırlamak için)
            currentRotationX = Mathf.Clamp(currentRotationX, -40f, 80f); // Alt ve üst sınır
        }

        // Kamerayı hesaplanan rotasyona göre yerleştir
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition; // Kameranın yeni pozisyonu
        transform.LookAt(target); // Kameranın oyuncuya bakmasını sağla
    }
}
