using UnityEngine;
using System;

public class LightController : MonoBehaviour
{
    public Light sunLight; // Directional Light'ı buraya atayın.
    public Gradient skyColor; // Gündüzden geceye gökyüzü rengini değiştirir.
    public AnimationCurve lightIntensity; // Güneş ışığının yoğunluğunu kontrol eder.

    void Update()
    {
        DateTime now = DateTime.Now;
        float hours = now.Hour + now.Minute / 60f; // Örn: 14.5
        float normalizedTime = hours / 24f; // 0-1 aralığında değer

        sunLight.color = skyColor.Evaluate(normalizedTime); // Gökyüzü rengini değiştir
        sunLight.intensity = lightIntensity.Evaluate(normalizedTime); // Işık yoğunluğunu ayarla

        RenderSettings.ambientLight = sunLight.color * 0.5f; // Ortam ışığını değiştir
    }
}
