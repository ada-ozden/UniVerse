using UnityEngine;
using UnityEngine.SceneManagement;
using UMA.CharacterSystem;

[RequireComponent(typeof(Collider))]
public class TriggerSceneTransition : MonoBehaviour
{
    [Header("Transition Ayarları")]
    [Tooltip("Build Settings'teki sahne adı")]
    public string targetSceneName = "InsideFacultyScene";

    private void OnTriggerEnter(Collider other)
    {
        // Sadece Player tag'li nesnelerde çalışsın
        if (!other.CompareTag("Player")) return;

        // 1) Player objesinin altındaki DynamicCharacterAvatar'ı al
        var avatar = other.GetComponentInChildren<DynamicCharacterAvatar>();
        if (avatar == null)
        {
            Debug.LogError("[TriggerSceneTransition] Player üzerinde DynamicCharacterAvatar bulunamadı!");
            return;
        }

        // 2) AvatarDefinition'ı sıkıştır ve PlayerPrefs'e kaydet
        var definition = avatar.GetAvatarDefinition(true);
        string compressed = definition.ToCompressedString("|");
        PlayerPrefs.SetString("SavedAvatar", compressed);
        PlayerPrefs.Save();
        Debug.Log("[TriggerSceneTransition] AvatarDefinition kaydedildi.");

        // 3) Sahne geçişi
        if (!string.IsNullOrEmpty(targetSceneName))
            SceneManager.LoadScene(targetSceneName);
        else
            Debug.LogError("[TriggerSceneTransition] targetSceneName boş!");
    }
}
