using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

public class LoginControl : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField sifreInputField;
    [SerializeField] private Button loginBtn;

    private FirebaseAuth auth;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

        if (loginBtn == null)
        {
            Debug.LogError("Button not assigned!");
        }
        else
        {
            loginBtn.onClick.AddListener(OnLoginButtonClicked);
            Debug.Log("Listener added to the button.");
        }
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void OnLoginButtonClicked()
    {
        Debug.Log("Giriş Yap button pressed");

        if (emailInputField == null || sifreInputField == null)
        {
            Debug.LogError("Email or Password Input Field is not assigned!");
            return;
        }

        string email = emailInputField.text;
        string sifre = sifreInputField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
        {
            Debug.LogWarning("Email veya Şifre boş olamaz.");
            return;
        }

        // Firebase ile oturum açma işlemini başlatın
        LoginUserFirebase(email, sifre);
    }

    private async void LoginUserFirebase(string email, string password)
    {
        if (auth == null)
        {
            Debug.LogError("Firebase auth is not initialized!");
            return;
        }

        try
        {
            var authResult = await auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseUser newUser = authResult.User;
            Debug.LogFormat("Firebase kullanıcı giriş başarılı: {0} ({1})", newUser.DisplayName, newUser.UserId);

            // E-posta doğrulamasını kontrol et
            if (newUser.IsEmailVerified)
            {
                // Kullanıcı adını AppManager'a ata
                AppManager.Instance.userName = newUser.DisplayName;
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            }
            else
            {
                Debug.LogWarning("Kullanıcının e-posta adresi doğrulanmamış.");
                Debug.LogWarning("Lütfen e-posta adresinizi doğrulayın.");
                await newUser.SendEmailVerificationAsync();
                Debug.Log("E-posta doğrulaması tekrar gönderildi: " + email);
            }
        }
        catch (FirebaseException e)
        {
            Debug.LogError("Firebase giriş hatası: " + e.Message);
        }
    }
}