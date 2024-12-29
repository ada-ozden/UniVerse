using UnityEngine;
using UnityEngine.UI;

public class CategoryUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject ThumbnailButtonPrefab; // Prefab for a thumbnail button
    [SerializeField] private Transform ScrollViewContent; // The "Content" object in the Scroll View

    [Header("Categories")]
    public ClothingCategory[] Categories; // Array of categories (Hair, Shoes, etc.)

    // Called when a category button is pressed (e.g., "Tops", "Shoes")
    public void OnCategoryButtonPressed(string categoryName)
{
    // Check if Categories array is null or empty
    if (Categories == null || Categories.Length == 0)
    {
        Debug.LogError("Categories array is not populated.");
        return;
    }

    // Clear existing thumbnails
    foreach (Transform child in ScrollViewContent)
    {
        Destroy(child.gameObject);
    }

    // Find the selected category
    bool categoryFound = false;  // Track if category is found
    foreach (var category in Categories)
    {
        if (category.CategoryName == categoryName)
        {
            categoryFound = true;

            // Check if the Items list is null or empty
            if (category.Items == null || category.Items.Count == 0)
            {
                Debug.LogWarning($"Category {categoryName} has no items.");
                continue;
            }

            // Populate thumbnails
            foreach (var item in category.Items)
            {
                if (item == null)
                {
                    Debug.LogWarning("Found null item in category.");
                    continue;
                }

                GameObject button = Instantiate(ThumbnailButtonPrefab, ScrollViewContent);
                if (button == null)
                {
                    Debug.LogError("Button prefab is not assigned properly.");
                    continue;
                }

                // Look for the specific child named "Thumbnail" under the button
                Transform thumbnailTransform = button.transform.Find("Thumbnail");

                if (thumbnailTransform != null)
                {
                    // Get the Image component on the "Thumbnail" child
                    Image buttonImage = thumbnailTransform.GetComponent<Image>();
                    if (buttonImage != null)
                    {
                        buttonImage.sprite = item.Thumbnail; // Assign the sprite
                        Debug.Log($"Thumbnail assigned: {buttonImage.sprite.name} for {item.Name}");
                    }
                    else
                    {
                    Debug.LogError("Image component not found on 'Thumbnail' child.");
                    }
                }
                else
                {
                    Debug.LogError("Child 'Thumbnail' not found in the button prefab.");
                }

                Button buttonComponent = button.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    buttonComponent.onClick.AddListener(() => PreviewItem(item));
                }
                else
                {
                    Debug.LogError("Button component not found in ThumbnailButtonPrefab.");
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


    // This method is called when an item button is pressed
    public void PreviewItem(ClothingItem item)
    {
        // For now, just log the name of the item, but you can replace this with your logic to apply the item
        Debug.Log($"Previewing {item.Name} (Slot: {item.SlotName}, Overlay: {item.OverlayName})");

        // For example, here you can apply the item to the character's UMA model:
        // ApplyClothingToCharacter(item);
    }

    // You can implement this method to apply the clothing item to your UMA character
    private void ApplyClothingToCharacter(ClothingItem item)
    {
        // Example: Use the SlotName and OverlayName to apply the item to your UMA character.
        // The actual implementation depends on how you're using UMA in your project.
        // Example (this is just a placeholder and may require adjusting):
        // UMACharacter.ApplyClothing(item.SlotName, item.OverlayName);
    }
}
