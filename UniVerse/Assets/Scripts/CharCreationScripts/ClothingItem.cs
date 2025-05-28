using UnityEngine;

[System.Serializable]
public class ClothingItem
{
    public string Name;          // Display name of the item
    public Sprite Thumbnail;     // Thumbnail for the UI
    public string SlotName;      // UMA slot name (e.g., "Torso")
    public string OverlayName;   // UMA overlay name
}
