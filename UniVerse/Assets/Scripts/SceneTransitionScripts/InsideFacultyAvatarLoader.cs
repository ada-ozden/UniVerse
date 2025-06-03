using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class InsideFacultyAvatarLoader : MonoBehaviour
{
    [Header("UMA References")]
    [Tooltip("InsideFaculty sahnesindeki DynamicCharacterAvatar objesini atayın.")]
    public DynamicCharacterAvatar avatar;

    private void Start()
    {
        LoadAvatarFromPrefs();
    }

    private void LoadAvatarFromPrefs()
    {
        // 1) PlayerPrefs'te "SavedAvatar" anahtarı var mı kontrol et
        if (!PlayerPrefs.HasKey("SavedAvatar"))
        {
            Debug.LogWarning("[InsideFacultyAvatarLoader] PlayerPrefs içinde 'SavedAvatar' bulunamadı; avatar yüklenemedi.");
            return;
        }

        string compressedString = PlayerPrefs.GetString("SavedAvatar");
        if (string.IsNullOrEmpty(compressedString))
        {
            Debug.LogWarning("[InsideFacultyAvatarLoader] 'SavedAvatar' stringi boş; avatar yüklenemedi.");
            return;
        }

        if (avatar == null)
        {
            Debug.LogError("[InsideFacultyAvatarLoader] 'avatar' referansı atanmamış! InsideFaculty sahnesindeki DynamicCharacterAvatar'ı atayın.");
            return;
        }

        // 2) Compressed string'i AvatarDefinition'a dönüştür
        AvatarDefinition adf = AvatarDefinition.FromCompressedString(compressedString, '|');

        // 3) UMA'ya uygula ve karakteri inşa et
        avatar.LoadAvatarDefinition(adf);
        avatar.BuildCharacter(false);

        Debug.Log("[InsideFacultyAvatarLoader] PlayerPrefs'ten alınan avatar başarılı şekilde yüklendi.");
    }
}