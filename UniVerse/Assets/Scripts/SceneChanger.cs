using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Butona tıklandığında çağrılacak fonksiyon
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene1");
    }
}