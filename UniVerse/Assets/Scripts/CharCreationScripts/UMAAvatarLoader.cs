using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using Firebase.Firestore;
using System.Threading.Tasks;

public class UMAAvatarLoader : MonoBehaviour
{
    [Header("UMA References")]
    public DynamicCharacterAvatar Avatar;  // Reference to the UMA avatar

    // Firestore
    private FirebaseFirestore firestore;

    private void Awake()
    {
        firestore = FirebaseFirestore.DefaultInstance;
    }

    private async void Start()
    {
        // Firestore’dan ilk olarak PlayerPrefs içindeki currentUserId’yi al
        await LoadAvatarFromFirestore();
    }

    /// <summary>
    /// PlayerPrefs’te saklı currentUserId’ye göre Firestore’dan avatarDefinition alanını çekip uygular.
    /// Eğer o ID’de bir belge bulunamazsa veya avatarDefinition yoksa PlayerPrefs’e kaylı “SavedAvatar” ile yedek yükleme yapar.
    /// </summary>
    private async Task LoadAvatarFromFirestore()
    {
        // 1) PlayerPrefs’ten “currentUserId” al
        if (!PlayerPrefs.HasKey("currentUserId"))
        {
            Debug.LogWarning("[UMAAvatarLoader] PlayerPrefs içinde 'currentUserId' bulunamadı. PlayerPrefs fallback yapılıyor.");
            LoadAvatarFromPrefs();
            return;
        }

        string savedUid = PlayerPrefs.GetString("currentUserId");
        if (string.IsNullOrEmpty(savedUid))
        {
            Debug.LogWarning("[UMAAvatarLoader] 'currentUserId' boş. PlayerPrefs fallback yapılıyor.");
            LoadAvatarFromPrefs();
            return;
        }

        // 2) Firestore’dan users/{savedUid} belgesini çek
        DocumentReference docRef = firestore
            .Collection("users")
            .Document(savedUid);

        try
        {
            DocumentSnapshot snap = await docRef.GetSnapshotAsync();
            if (snap.Exists && snap.ContainsField("avatarDefinition"))
            {
                string def = snap.GetValue<string>("avatarDefinition");
                if (!string.IsNullOrEmpty(def))
                {
                    ApplyAvatarDefinition(def);
                    return;
                }
            }
            // Eğer belge var ama “avatarDefinition” yoksa veya boşsa fallback
            Debug.LogWarning("[UMAAvatarLoader] Firestore’da avatarDefinition bulunamadı. PlayerPrefs fallback yapılıyor.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Firestore’dan avatar çekme hatası: " + e);
        }

        // 3) Fallback: PlayerPrefs içindeki “SavedAvatar” anahtarı ile yükleme yap
        LoadAvatarFromPrefs();
    }

    /// <summary>
    /// PlayerPrefs’te kayıtlı “SavedAvatar” stringi varsa, oradan yükleme yapar.
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
            Debug.LogWarning("[UMAAvatarLoader] PlayerPrefs içinde 'SavedAvatar' yok, hiç karakter yüklenemedi!");
        }
    }

    /// <summary>
    /// Compressed avatar string’i alıp UMA avatar’ı oluşturur.
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