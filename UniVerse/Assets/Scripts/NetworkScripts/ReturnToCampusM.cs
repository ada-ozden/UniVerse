using UnityEngine;
using Photon.Pun;

public class ReturnToCampusM : MonoBehaviourPunCallbacks
{
    [Tooltip("Exact name of your campus scene as in Build Settings")]
    public string campusSceneName = "campus";

    void Update()
    {
        // Listen for M key
        if (Input.GetKeyDown(KeyCode.M))
            ReturnToCampusScene();
    }

    /// <summary>
    /// You can still hook this up to a UI button if you like,
    /// but now it’s also called when you press M.
    /// </summary>
    public void OnExitBasketballPressed()
    {
        ReturnToCampusScene();
    }

    private void ReturnToCampusScene()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LoadLevel(campusSceneName);
        }
        else
        {
            Debug.LogWarning("Not in a Photon room — you’d have to rejoin first!");
            // fallback if needed:
            // UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
        }
    }
}
