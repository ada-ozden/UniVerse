using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    // Her butona bağlanacak fonksiyonlar
    public void GoToPreviousScene()
    {
        SceneManager.LoadScene("FirstScene");
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("GameScene1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("GameScene2 1");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("GameScene3 2");
    }
}