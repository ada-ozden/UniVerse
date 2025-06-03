using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class CampusAvatarLoader : MonoBehaviour
{
    [Header("UMA References")]
    [Tooltip("CampusScene’deki DynamicCharacterAvatar referansı")]
    public DynamicCharacterAvatar avatar;

    private void Start()
    {
        LoadAvatarFromPrefs();
    }

    private void LoadAvatarFromPrefs()
    {
        // PlayerPrefs’te “SavedAvatar” anahtarı var mı kontrol et
        if (!PlayerPrefs.HasKey("SavedAvatar"))
        {
            Debug.LogWarning("[CampusAvatarLoader] PlayerPrefs içinde 'SavedAvatar' bulunamadı. Avatar yüklenemedi.");
            return;
        }

        string compressedString = PlayerPrefs.GetString("SavedAvatar");
        if (string.IsNullOrEmpty(compressedString))
        {
            Debug.LogWarning("[CampusAvatarLoader] 'SavedAvatar' stringi boş. Avatar yüklenemedi.");
            return;
        }

        if (avatar == null)
        {
            Debug.LogError("[CampusAvatarLoader] 'avatar' referansı atanmadı! CampusScene içindeki DynamicCharacterAvatar’ı inspector’dan ekleyin.");
            return;
        }

        // 1) Compressed string’i AvatarDefinition’a dönüştür
        AvatarDefinition adf = AvatarDefinition.FromCompressedString(compressedString, '|');

        // 2) UMA’ya uygula ve karakteri inşa et
        avatar.LoadAvatarDefinition(adf);
        avatar.BuildCharacter(false);

        Debug.Log("[CampusAvatarLoader] PlayerPrefs’ten alınan avatar yüklenip inşa edildi.");
    }
}