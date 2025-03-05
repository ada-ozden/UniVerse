using UnityEngine;
using System;

public class SeasonBasedDaylight : MonoBehaviour
{
    public Light sunLight;

    void Update()
    {
        DateTime now = DateTime.Now;
        int month = now.Month;

        float sunrise = 6f; // Varsayılan gün doğumu
        float sunset = 18f; // Varsayılan gün batımı

        if (month >= 3 && month <= 5) // İlkbahar
        {
            sunrise = 5.5f;
            sunset = 19.5f;
        }
        else if (month >= 6 && month <= 8) // Yaz
        {
            sunrise = 5f;
            sunset = 21f;
        }
        else if (month >= 9 && month <= 11) // Sonbahar
        {
            sunrise = 6.5f;
            sunset = 18.5f;
        }
        else if (month == 12 || month <= 2) // Kış
        {
            sunrise = 7.5f;
            sunset = 17f;
        }

        float hours = now.Hour + now.Minute / 60f;
        float normalizedTime = Mathf.InverseLerp(sunrise, sunset, hours);

        sunLight.intensity = Mathf.Lerp(0, 1, normalizedTime);
    }
}
