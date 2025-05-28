using UnityEngine;

public class BuildingClick : MonoBehaviour
{
    public Sprite buildingPhoto; // Binaya ait fotoğraf

    void OnMouseDown() // Binaya tıklandığında
    {
        if (buildingPhoto != null)
        {
            BuildingPhotoManager.Instance.ShowPhoto(buildingPhoto);
        }
    }
}