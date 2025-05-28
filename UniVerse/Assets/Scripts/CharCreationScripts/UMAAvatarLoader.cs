using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using Firebase.Auth;
using Firebase.Firestore;
using System.Threading.Tasks;

public class UMAAvatarLoader : MonoBehaviour
{
    [Header("UMA References")]
    public DynamicCharacterAvatar Avatar;  // Reference to the UMA avatar

    // Firebase
    private FirebaseAuth auth;
    private FirebaseFirestore firestore;

    private void Awake()
    {
        // Initialize Firebase Auth & Firestore
        auth = FirebaseAuth.DefaultInstance;
        firestore = FirebaseFirestore.DefaultInstance;
    }

    private async void Start()
    {
        // Try loading from Firestore first; if that fails, fall back to PlayerPrefs
        await LoadAvatarFromFirestore();
    }

    /// <summary>
    /// Attempts to load the avatar definition from Firestore.
    /// If no definition is found or user is not signed in, falls back to PlayerPrefs.
    /// </summary>
    private async Task LoadAvatarFromFirestore()
    {
        var user = auth.CurrentUser;
        if (user != null)
        {
            DocumentReference docRef = firestore
                .Collection("users")
                .Document(user.UserId);
            try
            {
                DocumentSnapshot snap = await docRef.GetSnapshotAsync();
                if (snap.Exists && snap.ContainsField("avatarDefinition"))
                {
                    string def = snap.GetValue<string>("avatarDefinition");
                    ApplyAvatarDefinition(def);
                    return;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error retrieving avatar from Firestore: " + e);
            }
        }

        // Fallback
        LoadAvatarFromPrefs();
    }

    /// <summary>
    /// Loads the avatar definition string from PlayerPrefs.
    /// </summary>
    private void LoadAvatarFromPrefs()
    {
        if (PlayerPrefs.HasKey("SavedAvatar"))
        {
            string loadedAvatarString = PlayerPrefs.GetString("SavedAvatar");
            ApplyAvatarDefinition(loadedAvatarString);
        }
        else
        {
            Debug.LogWarning("No saved avatar found in PlayerPrefs!");
        }
    }

    /// <summary>
    /// Takes a compressed avatar string, reconstructs the AvatarDefinition,
    /// and builds the UMA character.
    /// </summary>
    /// <param name="compressedString">The compressed avatar definition string.</param>
    private void ApplyAvatarDefinition(string compressedString)
    {
        AvatarDefinition adf = AvatarDefinition
            .FromCompressedString(compressedString, '|');
        Avatar.LoadAvatarDefinition(adf);
        Avatar.BuildCharacter(false);
    }
}
