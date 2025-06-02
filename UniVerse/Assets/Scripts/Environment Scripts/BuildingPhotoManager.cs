using UnityEngine;
using UnityEngine.UI;

public class BuildingPhotoManager : MonoBehaviour
{
    public static BuildingPhotoManager Instance; // Diğer scriptlerden erişmek için

    public GameObject photoPanel; // Fotoğraf paneli
    public Image photoDisplay;   // Fotoğrafın gösterileceği Image

    void Awake()
    {
        Instance = this; // Singleton yapısı
    }

    public void ShowPhoto(Sprite photo)
    {
        photoDisplay.sprite = photo; // Fotoğrafı Image'a yerleştir
        photoPanel.SetActive(true);   // Paneli aç
    }

    public void ClosePhoto()
    {
        photoPanel.SetActive(false); // Paneli kapat
    }
}