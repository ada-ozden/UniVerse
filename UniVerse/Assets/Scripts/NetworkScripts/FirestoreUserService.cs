using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirestoreUserService : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseFirestore db;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
    }

    public async Task<string> GetCurrentUserKullaniciAdiAsync()
    {
        var user = auth.CurrentUser;
        if (user == null)
        {
            Debug.LogError("Oturum açmış kullanıcı yok.");
            return null;
        }

        try
        {
            var snap = await db.Collection("users")
                               .Document(user.UserId)
                               .GetSnapshotAsync();
            if (snap.Exists && snap.ContainsField("kullaniciAdi"))
                return snap.GetValue<string>("kullaniciAdi");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Firestore hatası: " + e);
        }
        return null;
    }
}
