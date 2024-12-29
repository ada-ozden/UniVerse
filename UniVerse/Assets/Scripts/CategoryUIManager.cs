using UnityEngine;
using UnityEngine.UI;
using UMA;
using UMA.CharacterSystem;

public class CategoryUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject ThumbnailButtonPrefab;
    [SerializeField] private Transform ScrollViewContent;

    [Header("Categories")]
    public ClothingCategory[] Categories;

    [Header("UMA References")]
    [SerializeField] private DynamicCharacterAvatar avatar; // Reference to the UMA character

    public void OnCategoryButtonPressed(string categoryName)
    {
        if (Categories == null || Categories.Length == 0)
        {
            Debug.LogError("Categories array is not populated.");
            return;
        }

        foreach (Transform child in ScrollViewContent)
        {
            Destroy(child.gameObject);
        }

        bool categoryFound = false;
        foreach (var category in Categories)
        {
            if (category.CategoryName == categoryName)
            {
                categoryFound = true;

                if (category.Items == null || category.Items.Count == 0)
                {
                    Debug.LogWarning($"Category {categoryName} has no items.");
                    continue;
                }

                foreach (var item in category.Items)
                {
                    if (item == null) continue;

                    GameObject button = Instantiate(ThumbnailButtonPrefab, ScrollViewContent);
                    Transform thumbnailTransform = button.transform.Find("Thumbnail");

                    if (thumbnailTransform != null)
                    {
                        Image buttonImage = thumbnailTransform.GetComponent<Image>();
                        if (buttonImage != null)
                        {
                            buttonImage.sprite = item.Thumbnail;
                        }
                    }

                    Button buttonComponent = button.GetComponent<Button>();
                    if (buttonComponent != null)
                    {
                        buttonComponent.onClick.AddListener(() => PreviewItem(item));
                    }
                }
                break;
            }
        }

        if (!categoryFound)
        {
            Debug.LogWarning($"Category {categoryName} not found.");
        }
    }

    public void PreviewItem(ClothingItem item)
    {
        Debug.Log($"Previewing {item.Name} (Slot: {item.SlotName}, Overlay: {item.OverlayName})");
        ApplyClothingToCharacter(item);
    }

    private void ApplyClothingToCharacter(ClothingItem item)
    {
        if (avatar == null)
        {
            Debug.LogError("DynamicCharacterAvatar is not assigned.");
            return;
        }

        // Remove the previous slot and apply the new one
        avatar.SetSlot(item.SlotName, item.OverlayName);

        // Update the UMA character to apply the changes
        avatar.BuildCharacter();
    }
}

