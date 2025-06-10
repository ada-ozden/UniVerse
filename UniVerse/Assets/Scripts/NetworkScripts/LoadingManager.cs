using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using Photon.Pun;
using System.Threading.Tasks;

public class LoadingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string lobbySceneName = "Lobby";

    FirebaseAuth auth;
    FirebaseFirestore db;

    async void Start()
    {
        // Firebase setup
        auth = FirebaseAuth.DefaultInstance;
        db   = FirebaseFirestore.DefaultInstance;

        // 1. Oturumu açık kullanıcıyı al, UID’siyle Firestore’dan dokümanı çek
        var user = auth.CurrentUser;
        string userName = "Guest"; 

        if (user != null)
        {
            try
            {
                var snap = await db.Collection("users")
                                   .Document(user.UserId)
                                   .GetSnapshotAsync();

                if (snap.Exists && snap.ContainsField("kullaniciAdi"))
                    userName = snap.GetValue<string>("kullaniciAdi");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Firestore hatası: " + e);
            }
        }
        else
        {
            Debug.LogWarning("Oturum açmış kullanıcı yok, Guest olarak bağlanılıyor.");
        }

        // 2. Photon NickName olarak ata ve bağlan
        PhotonNetwork.NickName = userName;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // 3. Lobby sahnesine geç
        PhotonNetwork.LoadLevel(lobbySceneName);
    }
}
