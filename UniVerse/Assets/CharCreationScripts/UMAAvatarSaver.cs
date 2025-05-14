using UnityEngine;
using UnityEngine.UI;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.SceneManagement;

public class UMAAvatarSaver : MonoBehaviour
{
    public DynamicCharacterAvatar Avatar;  // Reference to the UMA avatar
    public Button saveButton;              // Save button

    private void Start()
    {
        // Add a listener to the save button
        saveButton.onClick.AddListener(SaveAvatarAndChangeScene);
    }

    // Method to save the avatar and load the new scene
    private void SaveAvatarAndChangeScene()
    {
        // Get avatar definition and save it as a compressed string
        string savedAvatarString = Avatar.GetAvatarDefinition(true).ToCompressedString("|");

        // Store the saved string in PlayerPrefs
        PlayerPrefs.SetString("SavedAvatar", savedAvatarString);
        PlayerPrefs.Save();

        // Load the new scene (replace "NewSceneName" with the actual scene name)
        SceneManager.LoadScene("Example");
    }

    // Method to load the avatar in the new scene
    public void LoadAvatar()
    {
        if (PlayerPrefs.HasKey("SavedAvatar"))
        {
            // Retrieve the saved string
            string loadedAvatarString = PlayerPrefs.GetString("SavedAvatar");

            // Convert the string back to an AvatarDefinition
            AvatarDefinition adf = AvatarDefinition.FromCompressedString(loadedAvatarString, '|');

            // Load the avatar definition into the character
            Avatar.LoadAvatarDefinition(adf);
            Avatar.BuildCharacter(false);  // Build the character without restoring old DNA
        }
    }
}
