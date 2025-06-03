using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.SceneManagement;

public class AvatarSceneTransition : MonoBehaviour
{
    [Header("UMA References")]
    [Tooltip("LobbyScene’deki DynamicCharacterAvatar referansı")]
    [SerializeField] private DynamicCharacterAvatar avatar; 

    [Header("Scene Names")]
    [Tooltip("Yönlendirme yapılacak CampusScene adı (Build Settings ile aynı olmalı)")]
    public string campusSceneName = "campus";

    private void Update()
    {
        // F tuşuna basıldığında sadece bir kere tetiklemek için GetKeyDown kullanıyoruz
        if (Input.GetKeyDown(KeyCode.F))
        {
            SendAvatarToCampus();
        }
    }

    private void SendAvatarToCampus()
    {
        if (avatar == null)
        {
            Debug.LogError("[AvatarSceneTransition] 'avatar' referansı atanmadı! LobbyScene içerisindeki DynamicCharacterAvatar’ı inspector’dan ekleyin.");
            return;
        }

        // 1) Mevcut UMA avatar tanımını sıkıştırılmış (compressed) string olarak al
        AvatarDefinition adf = avatar.GetAvatarDefinition(true);
        string compressedString = adf.ToCompressedString("|");

        // 2) PlayerPrefs’e "SavedAvatar" anahtarıyla kaydet
        PlayerPrefs.SetString("SavedAvatar", compressedString);
        PlayerPrefs.Save();
        Debug.Log("[AvatarSceneTransition] AvatarDefinition PlayerPrefs’e kaydedildi.");

        // 3) CampusScene’e geçiş yap
        if (!string.IsNullOrEmpty(campusSceneName))
        {
            SceneManager.LoadScene(campusSceneName);
        }
        else
        {
            Debug.LogError("[AvatarSceneTransition] 'campusSceneName' boş! CampusScene adını script inspector’ından girin.");
        }
    }
}