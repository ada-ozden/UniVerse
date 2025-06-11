using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ReturnToCampus : MonoBehaviourPunCallbacks
{
    [Tooltip("Exact name of your campus scene as in Build Settings")]
    public string campusSceneName = "campus";

    /// <summary>
    /// Hook this up to your “Exit Basketball” button’s OnClick().
    /// </summary>
    public void OnExitBasketballPressed()
    {
        // If we’re still in the same Photon room, just load the campus scene
        if (PhotonNetwork.InRoom)
        {
            // This will keep your room/lobby connection alive
            PhotonNetwork.LoadLevel(campusSceneName);
        }
        else
        {
            Debug.LogWarning("Not in a Photon room — you’d have to rejoin first!");
            // fallback if you really have lost connection:
            SceneManager.LoadScene("Loading");
        }
    }
}
