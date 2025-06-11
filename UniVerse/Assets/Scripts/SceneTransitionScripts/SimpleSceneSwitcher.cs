using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneSwitcher : MonoBehaviour
{
    // Call this (e.g. from a UI Button) with the exact scene name:
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}