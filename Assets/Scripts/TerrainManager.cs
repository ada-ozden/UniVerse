using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public Terrain terrain;

    // Her mevsim için TerrainLayer referansları
    public TerrainLayer winterLayer, springLayer, summerLayer, autumnLayer;

    void Start()
    {
        // Başlangıçta terrain dokusunu güncelle
        if (terrain != null)
        {
            terrain.terrainData.terrainLayers = new TerrainLayer[] { winterLayer }; // Başlangıçta kış dokusunu ayarlıyoruz
        }
    }

    // Mevsime göre terrain dokusunu güncelleyen fonksiyon
    public void UpdateTerrainTexture(SeasonManager.Season currentSeason)
    {
        switch (currentSeason)
        {
            case SeasonManager.Season.Winter:
                SetTerrainLayer(winterLayer);
                break;
            case SeasonManager.Season.Spring:
                SetTerrainLayer(springLayer);
                break;
            case SeasonManager.Season.Summer:
                SetTerrainLayer(summerLayer);
                break;
            case SeasonManager.Season.Autumn:
                SetTerrainLayer(autumnLayer);
                break;
        }
    }

    // TerrainLayer'ı terrain'e uygulama
    void SetTerrainLayer(TerrainLayer layer)
    {
        terrain.terrainData.terrainLayers = new TerrainLayer[] { layer };
    }
}