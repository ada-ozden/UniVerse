using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class UMAAvatarLoader : MonoBehaviour
{
    public DynamicCharacterAvatar Avatar;  // Reference to the UMA avatar

    private void Start()
    {
        LoadAvatar();
    }

    private void LoadAvatar()
    {
        // Check if a saved avatar string exists in PlayerPrefs
        if (PlayerPrefs.HasKey("SavedAvatar"))
        {
            // Retrieve the saved string from PlayerPrefs
            string loadedAvatarString = PlayerPrefs.GetString("SavedAvatar");

            // Convert the string back to an AvatarDefinition
            AvatarDefinition adf = AvatarDefinition.FromCompressedString(loadedAvatarString, '|');

            // Load the avatar definition into the character
            Avatar.LoadAvatarDefinition(adf);

            // Build the character without restoring old DNA
            Avatar.BuildCharacter(false);
        }
        else
        {
            Debug.LogWarning("No saved avatar found in PlayerPrefs!");
        }
    }
}
