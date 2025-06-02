using UnityEngine;

public class SunController : MonoBehaviour
{
    public Transform sunTransform;    // Güneşin Transform bileşeni (Directional Light objesinin Transform'u)
    public float dayDuration = 24f;   // Gün süresi, gerçek zamanla uyumlu (24 saat)
 
    void Update()
    {
        // Gerçek saati al (0.0 - 1.0 arasında bir değer)
        float timeOfDay = (float)System.DateTime.Now.Hour / 24f;
     
        // Güneşi döndür (0-360 derece arası)
        float sunRotation = timeOfDay * 360f;

        // Güneşi Y ekseninde döndür
        sunTransform.rotation = Quaternion.Euler(sunRotation, 170f, 0f); // 170f sabit açı, yönü değiştir
    }
}