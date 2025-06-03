using UnityEngine;
using UnityEngine.SceneManagement;
using UMA.CharacterSystem;

public class BackToCampusTrigger : MonoBehaviour
{
    [Header("UMA Avatar")]
    [Tooltip("InsideFaculty sahnesinde hâlihazırda yüklü UMA avatarınızı buraya atayın.")]
    public DynamicCharacterAvatar avatar;

    [Header("Scene Settings")]
    [Tooltip("Geri dönmek istediğiniz Campus sahnesinin tam adı (Build Settings ile aynı).")]
    public string campusSceneName = "CampusScene";

    private void OnTriggerEnter(Collider other)
    {
        // Sadece tag'i "Player" olan nesne (yani avatar) tetiklesin
        if (!other.CompareTag("Player"))
            return;

        // 1) Eğer avatar tanımı değişmişse (örneğin yüklenirken bazı öğrenci objeleri değiştiysa) tekrar kaydet
        if (avatar != null)
        {
            AvatarDefinition adf = avatar.GetAvatarDefinition(true);
            string compressedString = adf.ToCompressedString("|");
            PlayerPrefs.SetString("SavedAvatar", compressedString);
            PlayerPrefs.Save();
            Debug.Log("[BackToCampusTrigger] AvatarDefinition PlayerPrefs’e tekrar kaydedildi.");
        }
        else
        {
            Debug.LogWarning("[BackToCampusTrigger] 'avatar' referansı atanmamış! Inspector’dan ekleyin.");
        }

        // 2) Campus sahnesine geçiş yap
        if (!string.IsNullOrEmpty(campusSceneName))
        {
            SceneManager.LoadScene(campusSceneName);
        }
        else
        {
            Debug.LogError("[BackToCampusTrigger] campusSceneName boş bırakılmış!");
        }
    }
}