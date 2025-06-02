using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateScreen1 : MonoBehaviour
{
    public void GoToPreviousScene()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene("GameScene2 1");
    }
}