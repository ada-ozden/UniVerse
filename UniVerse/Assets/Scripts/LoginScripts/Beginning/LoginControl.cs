using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Firestore;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.SceneManagement;

public class LoginControl : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField sifreInputField;
    [SerializeField] private Button loginBtn;
    [SerializeField] private TextMeshProUGUI emailErrorText;
    [SerializeField] private TextMeshProUGUI emailControlErrorText;
    [SerializeField] private TextMeshProUGUI sifreErrorText;
    [SerializeField] private TextMeshProUGUI sifreControlErrorText;
     [Header("Scene Names")]
    [SerializeField] private string characterCreationScene = "CharCreator";
    [SerializeField] private string characterCreationSceneMale = "CharCreatorMale";

    private FirebaseFirestore firestore;

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

        ResetErrorTexts();
    }

    void InitializeFirebase()
    {
        firestore = FirebaseFirestore.DefaultInstance;
    }

    public void OnLoginButtonClicked()
    {
        Debug.Log("Giriş Yap button pressed");

        // Hata mesajlarını sıfırlama
        ResetErrorTexts();

        if (emailInputField == null || sifreInputField == null)
        {
            Debug.LogError("Email or Password Input Field is not assigned!");
            return;
        }

        string email = emailInputField.text.Trim();
        string sifre = sifreInputField.text.Trim();

        if (string.IsNullOrEmpty(email))
        {
            emailErrorText.text = "Email bos olamaz.";
            emailErrorText.gameObject.SetActive(true);
           
            return;
        }

        if (string.IsNullOrEmpty(sifre))
        {
            sifreErrorText.text = "Sifre bos olamaz.";
            sifreErrorText.gameObject.SetActive(true);
            
            return;
        }

        // Firestore ile oturum açma işlemini başlatın
        LoginUserFirestore(email, sifre);
    }

    private async void LoginUserFirestore(string email, string password)
    {
        try
        {
            // Firestore'dan email ile kullanıcıyı sorgula
            Query userQuery = firestore.Collection("users").WhereEqualTo("kullaniciAdi", email);
            QuerySnapshot userQuerySnapshot = await userQuery.GetSnapshotAsync();

            // İlk belgeyi al
            DocumentSnapshot userDoc = userQuerySnapshot.Documents.FirstOrDefault();

            if (userDoc == null)
            {
                emailControlErrorText.text = "E-posta adresi hatali.";
                emailControlErrorText.gameObject.SetActive(true);
                Debug.LogError("Kullanıcı bulunamadı.");
                return;
            }

            // Veritabanından hashlenmiş şifreyi al
            string hashedPasswordInDb = userDoc.GetValue<string>("hashedPassword");

            // Kullanıcının girdiği şifreyi hashle
            string hashedPasswordInput = PasswordHasher.HashPassword(password);

            // Hashlenmiş şifreleri karşılaştır
            if (hashedPasswordInDb == hashedPasswordInput)
            {
                Debug.Log("Şifre doğru, giriş başarılı.");
                //SceneManager.LoadScene(characterCreationScene);
                // Firestore'dan cinsiyeti oku
                string gender = userDoc.Exists && userDoc.ContainsField("cinsiyet")
                    ? userDoc.GetValue<string>("cinsiyet")
                    : "";

                // Cinsiyete göre sahne yükle
                if (gender.Equals("Kadın", System.StringComparison.OrdinalIgnoreCase))
                    SceneManager.LoadScene(characterCreationScene);
                else if (gender.Equals("Erkek", System.StringComparison.OrdinalIgnoreCase))
                    SceneManager.LoadScene(characterCreationSceneMale);
                else
                    Debug.LogWarning("Cinsiyet alanı bulunamadı veya tanınmıyor: " + gender);
            }
            else
            {
                sifreControlErrorText.text = "Sifre hatali.";
                sifreControlErrorText.gameObject.SetActive(true);  // Hata mesajını görünür yap

            }
        }
        catch (FirebaseException e)
        {
            Debug.LogError("Firebase giriş hatası: " + e.Message);
            emailErrorText.text = "Firebase giriş hatası: " + e.Message;
            emailErrorText.gameObject.SetActive(true);
        }
    }

    private void ResetErrorTexts()
    {
        emailErrorText.text = "";
        emailErrorText.gameObject.SetActive(false);

        emailControlErrorText.text = "";
        emailControlErrorText.gameObject.SetActive(false);

        sifreErrorText.text = "";
        sifreErrorText.gameObject.SetActive(false);

        sifreControlErrorText.text = "";
        sifreControlErrorText.gameObject.SetActive(false);
    }
}

// Şifre hashleme için hasher sınıfı
public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var builder = new System.Text.StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
