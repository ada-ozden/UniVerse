 using UnityEngine;
using UnityEngine.UI;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UMAAvatarSaver : MonoBehaviour
{
    [Header("UMA References")]
    public DynamicCharacterAvatar Avatar;      // UMA avatar reference

    [Header("UI References")]
    public Button saveButton;                  // Button to trigger save + scene change
    [Tooltip("Karakter kaydından sonra yönlendirilecek sahne adı")]
    public string nextSceneName = "LobbyScene";

    // Firebase
    private FirebaseAuth auth;
    private FirebaseFirestore firestore;

    private void Awake()
    {
        // Firebase Auth & Firestore init
        auth = FirebaseAuth.DefaultInstance;
        firestore = FirebaseFirestore.DefaultInstance;
    }

    private void Start()
    {
        if (saveButton != null)
            saveButton.onClick.AddListener(SaveAvatarAndChangeScene);
        else
            Debug.LogWarning("[UMAAvatarSaver] Save Button inspector’da atanmadı!");
    }

    private async void SaveAvatarAndChangeScene()
    {
        // --- 1) UMA dizgesini hazırla ---
        string avatarString = Avatar
            .GetAvatarDefinition(true)
            .ToCompressedString("|");

        // --- 2) Lokal kaydet (isteğe bağlı) ---
        PlayerPrefs.SetString("SavedAvatar", avatarString);
        PlayerPrefs.Save();

        // --- 3) Firestore’a kaydet: users/{uid} dokümanına merge ile ekle/güncelle ---
        var user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log($"[UMAAvatarSaver] Saving avatar for userId = {user.UserId}");
            var docRef = firestore
                .Collection("users")
                .Document(user.UserId);

            var data = new Dictionary<string, object>
            {
                { "avatarDefinition", avatarString },
                { "avatarSavedAt", Timestamp.GetCurrentTimestamp() }
            };

            try
            {
                // Doküman yoksa oluşturur, varsa sadece bu alanları birleştirir
                await docRef.SetAsync(data, SetOptions.MergeAll);
                Debug.Log("[UMAAvatarSaver] Avatar definition successfully saved to Firestore.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[UMAAvatarSaver] Error saving avatar to Firestore: {e}");
            }
        }
        else
        {
            Debug.LogWarning("[UMAAvatarSaver] No authenticated user found. Avatar not saved.");
        }

        // --- 4) Sonraki sahneye geç ---
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("[UMAAvatarSaver] nextSceneName boş, sahneye geçiş yapılmadı.");
    }

    /// <summary>
    /// Prefs’ten yükleme için (fallback veya test amaçlı).
    /// </summary>
    public void LoadAvatar()
    {
        if (PlayerPrefs.HasKey("SavedAvatar"))
        {
            string loadedAvatarString = PlayerPrefs.GetString("SavedAvatar");
            AvatarDefinition adf = AvatarDefinition
                .FromCompressedString(loadedAvatarString, '|');
            Avatar.LoadAvatarDefinition(adf);
            Avatar.BuildCharacter(false);
        }
        else
        {
            Debug.LogWarning("[UMAAvatarSaver] No SavedAvatar key in PlayerPrefs!");
        }
    }
}