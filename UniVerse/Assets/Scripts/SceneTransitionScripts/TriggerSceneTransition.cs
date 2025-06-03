using UnityEngine;
using UnityEngine.SceneManagement;
using UMA.CharacterSystem; // AvatarDefinition için gerekli

public class TriggerSceneTransition : MonoBehaviour
{
    [Header("UMA Avatar")]
    [Tooltip("Bu alana LobbyScene'deki DynamicCharacterAvatar objesini atayın.")]
    public DynamicCharacterAvatar avatar;

    [Header("Transition Settings")]
    [Tooltip("Geçecek sahnenin adı (Build Settings'teki tam adı).")]
    public string targetSceneName = "InsideFaculty";

    private void OnTriggerEnter(Collider other)
    {
        // Sadece tag'i "Player" olan nesne tetikleyiciyi aktifleştirsin
        if (!other.CompareTag("Player"))
            return;

        // 1) Avatar'a compress edilmiş string'i al
        if (avatar == null)
        {
            Debug.LogError("[TriggerSceneTransition] 'avatar' referansı atanmamış!");
            return;
        }

        // Mevcut avatarDefinition'ı sıkıştırılmış string olarak elde et
        AvatarDefinition adf = avatar.GetAvatarDefinition(true);
        string compressedString = adf.ToCompressedString("|");

        // 2) PlayerPrefs'e kaydet
        PlayerPrefs.SetString("SavedAvatar", compressedString);
        PlayerPrefs.Save();
        Debug.Log("[TriggerSceneTransition] AvatarDefinition PlayerPrefs'e kaydedildi.");

        // 3) InsideFaculty sahnesine geçiş yap
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("[TriggerSceneTransition] 'targetSceneName' boş bırakılmış!");
        }
    }
}