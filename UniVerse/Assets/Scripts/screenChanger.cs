using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

public class SceneChanger : MonoBehaviour
{
    public RectTransform animationPanel;
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Belirtilen sahneyi yükler
    }
}
