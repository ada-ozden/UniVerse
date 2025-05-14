using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    public string nextSceneName;

    public void NextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
