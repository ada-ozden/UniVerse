using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    // Her butona bağlanacak fonksiyonlar
    public void LoadLevel1()
    {
        SceneManager.LoadScene("GameScene1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("GameScene2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("GameScene3");
    }
}