using Firebase;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Firebase bağımlılıklarını kontrol et
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Bağlantı başarılı
                Debug.Log("Firebase bağlantısı başarılı!");
            }
            else
            {
                // Bağlantı hatalı
                Debug.LogError("Firebase bağlantısı başarısız: " + dependencyStatus.ToString());
            }
        });
    }
}
