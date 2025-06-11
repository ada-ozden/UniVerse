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
    [SerializeField] private string CharCreator = "CharCreator";
    [SerializeField] private string CharCreatorMale = "CharCreatorMale";

    private FirebaseFirestore firestore;

    private void Start()
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

    private void InitializeFirebase()
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
            Debug.LogError("Email veya Şifre Input Field’ı atanmadı!");
            return;
        }

        string email = emailInputField.text.Trim();
        string sifre = sifreInputField.text.Trim();

        if (string.IsNullOrEmpty(email))
        {
            emailErrorText.text = "Email boş olamaz.";
            emailErrorText.gameObject.SetActive(true);
            return;
        }

        if (string.IsNullOrEmpty(sifre))
        {
            sifreErrorText.text = "Şifre boş olamaz.";
            sifreErrorText.gameObject.SetActive(true);
            return;
        }

        // Firestore üzerinden “kullaniciAdi” alanı ile kullanıcıyı sorgula
        LoginUserFirestore(email, sifre);
    }

    private async void LoginUserFirestore(string email, string password)
    {
        try
        {
            // 1) users koleksiyonunda, “kullaniciAdi” = email olan belgeyi getir
            Query userQuery = firestore.Collection("users").WhereEqualTo("kullaniciAdi", email);
            QuerySnapshot userQuerySnapshot = await userQuery.GetSnapshotAsync();

            DocumentSnapshot userDoc = userQuerySnapshot.Documents.FirstOrDefault();
            if (userDoc == null)
            {
                // Kullanıcı bulunamadı
                emailControlErrorText.text = "E-posta adresi hatalı.";
                emailControlErrorText.gameObject.SetActive(true);
                Debug.LogError("Kullanıcı bulunamadı.");
                return;
            }

            // 2) Veritabanındaki hashlenmiş şifreyi al
            string hashedPasswordInDb = userDoc.GetValue<string>("hashedPassword");

            // 3) Kullanıcının girdiği şifreyi hashle
            string hashedPasswordInput = PasswordHasher.HashPassword(password);

            // 4) Şifreleri karşılaştır
            if (hashedPasswordInDb != hashedPasswordInput)
            {
                sifreControlErrorText.text = "Şifre hatalı.";
                sifreControlErrorText.gameObject.SetActive(true);
                return;
            }

            // Şifre doğruysa:
            Debug.Log("Şifre doğru, giriş başarılı.");

            // 5) Doğru kullanıcı ID’sini PlayerPrefs’e kaydet
            string uid = userDoc.Id; 
            PlayerPrefs.SetString("currentUserId", uid);
            PlayerPrefs.Save();
            Debug.Log($"[LoginControl] currentUserId '{uid}' olarak PlayerPrefs’e kaydedildi.");

            // 6) Kullanıcının Firestore’daki “avatarDefinition” var mı diye kontrol et
            bool characterExists = false;
            if (userDoc.ContainsField("avatarDefinition"))
            {
                string avatarDef = userDoc.GetValue<string>("avatarDefinition");
                if (!string.IsNullOrEmpty(avatarDef))
                    characterExists = true;
            }

            // 7) Sahne yönlendirme
            if (characterExists)
            {
                // Karakter varsa direkt Lobby’e geç
                SceneManager.LoadScene("Loading");
            }
            else
            {
                // Karakter yoksa cinsiyete göre CharCreator sahnesine yönlendir
                string gender = userDoc.ContainsField("cinsiyet") 
                    ? userDoc.GetValue<string>("cinsiyet") 
                    : "";

                if (gender.Equals("Kadın", System.StringComparison.OrdinalIgnoreCase))
                {
                    SceneManager.LoadScene(CharCreator);
                }
                else if (gender.Equals("Erkek", System.StringComparison.OrdinalIgnoreCase))
                {
                    SceneManager.LoadScene(CharCreatorMale);
                }
                else
                {
                    Debug.LogWarning($"[LoginControl] Cinsiyet alanı bulunamadı veya tanınmıyor: '{gender}', varsayılan olarak CharCreator’a geçiliyor.");
                    SceneManager.LoadScene(CharCreator);
                }
            }
        }
        catch (FirebaseException e)
        {
            Debug.LogError("Firestore login hatası: " + e.Message);
            emailErrorText.text = "Giriş hatası: " + e.Message;
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

// Şifre hash’lemek için yardımcı sınıf
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