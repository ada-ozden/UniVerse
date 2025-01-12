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
        foreach (Transform child in ScrollViewContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var category in Categories)
        {
            if (category.CategoryName == categoryName)
            {
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
    }

    public void PreviewItem(ClothingItem item)
    {
        ApplyClothingToCharacter(item);
    }

    private void ApplyClothingToCharacter(ClothingItem item)
    {
        // Remove the previous slot and apply the new one
        avatar.SetSlot(item.SlotName, item.OverlayName);

        // Update the UMA character to apply the changes
        avatar.BuildCharacter();
    }
}

