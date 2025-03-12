using System;
using UnityEngine;

public enum Season { Winter, Spring, Summer, Autumn }

public class SeasonManager : MonoBehaviour
{
    public Season currentSeason;
    public TerrainManager terrainManager;  // TerrainManager referansı

    void Start()
    {
        // Başlangıçta mevsimi belirle
        currentSeason = GetSeasonFromDate(DateTime.Now);
        UpdateTerrain();  // Başlangıçta terrain'i güncelle
    }

    void Update()
    {
        // Her frame'de güncel tarih üzerinden mevsimi kontrol et
        Season newSeason = GetSeasonFromDate(DateTime.Now);

        // Mevsim değiştiyse, güncelleme yap
        if (newSeason != currentSeason)
        {
            currentSeason = newSeason;
            UpdateTerrain();  // Terrain dokusunu güncelle
        }
    }

    // Tarihe göre mevsimi döndüren fonksiyon
    Season GetSeasonFromDate(DateTime date)
    {
        int month = date.Month;

        // Mevsimlerin belirlenmesi
        if (month == 12 || month <= 2) // Kış (Aralık, Ocak, Şubat)
        {
            return Season.Winter;
        }
        else if (month >= 3 && month <= 5) // İlkbahar (Mart, Nisan, Mayıs)
        {
            return Season.Spring;
        }
        else if (month >= 6 && month <= 8) // Yaz (Haziran, Temmuz, Ağustos)
        {
            return Season.Summer;
        }
        else // Sonbahar (Eylül, Ekim, Kasım)
        {
            return Season.Autumn;
        }
    }

    // TerrainManager'a mevsimi ileterek terrain dokusunu günceller
    void UpdateTerrain()
    {
        if (terrainManager != null)
        {
            terrainManager.UpdateTerrainTexture(currentSeason);
        }
    }
}