using UnityEngine;
using UnityEngine.SceneManagement;
public class NavigateScreen2 : MonoBehaviour
{
    public void GoToPreviousScene()
    {
        SceneManager.LoadScene("FirstScene");
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene("GameScene3 1");
    }
}
