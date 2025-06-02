using UnityEngine;
using UnityEngine.SceneManagement;
public class NavigateScreen2 : MonoBehaviour
{
    public void GoToPreviousScene()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene("GameScene3 2");
    }
}
