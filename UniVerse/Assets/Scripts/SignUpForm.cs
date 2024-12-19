using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System.Threading.Tasks;

public class SignUpForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField adInputField;
    [SerializeField] private TMP_InputField soyadInputField;
    [SerializeField] private TMP_InputField kullaniciAdiInputField;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField sifreInputField;
    [SerializeField] private Toggle kadinToggle;
    [SerializeField] private Toggle erkekToggle;
    [SerializeField] private TMP_Dropdown bolumDropdown;
    [SerializeField] private Button kaydolBtn;
    [SerializeField] private TextMeshProUGUI generalErrorText;

    private FirebaseAuth auth;
    private FirebaseFirestore firestore;
    private bool isSubmitting = false;

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

        if (kaydolBtn == null)
        {
            Debug.LogError("Button not assigned!");
        }
        else
        {
            kaydolBtn.onClick.AddListener(OnSignUpButtonClicked);
            Debug.Log("Listener added to the button.");
        }
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firestore = FirebaseFirestore.DefaultInstance;
    }

    public void OnSignUpButtonClicked()
    {
        if (isSubmitting) return;

        generalErrorText.text = "";
        generalErrorText.gameObject.SetActive(false);

        string ad = adInputField.text;
        string soyad = soyadInputField.text;
        string kullaniciAdi = kullaniciAdiInputField.text;
        string email = emailInputField.text;
        string sifre = sifreInputField.text;
        string cinsiyet = kadinToggle.isOn ? "Kadın" : erkekToggle.isOn ? "Erkek" : "";
        string bolum = bolumDropdown.options[bolumDropdown.value].text;

        StringBuilder errorBuilder = new StringBuilder();
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(ad))
        {
            isValid = false;
            errorBuilder.AppendLine("Ad alani bos olamaz.");
        }
        if (string.IsNullOrWhiteSpace(soyad))
        {
            isValid = false;
            errorBuilder.AppendLine("Soyad alani bos olamaz.");
        }
        if (!IsValidUsername(kullaniciAdi))
        {
            isValid = false;
            errorBuilder.AppendLine("Kullanici adi 3-15 karakter uzunlugunda olmali.");
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            isValid = false;
            errorBuilder.AppendLine("E-posta alani bos olamaz.");
        }
        else if (!IsValidEmail(email))
        {
            isValid = false;
            errorBuilder.AppendLine("Gecerli bir e-posta adresi girin.");
        }
        if (!IsValidPassword(sifre))
        {
            isValid = false;
            errorBuilder.AppendLine("Sifre 8-16 karakter arasinda olmali ve en az bir buyuk harf, bir kucuk harf, bir sayi ve bir ozel karakter icermelidir.");
        }
        if (string.IsNullOrWhiteSpace(cinsiyet))
        {
            isValid = false;
            errorBuilder.AppendLine("Cinsiyet secimi zorunludur.");
        }

        if (!isValid)
        {
            generalErrorText.text = errorBuilder.ToString();
            generalErrorText.gameObject.SetActive(true);
            return;
        }

        isSubmitting = true;

        var userData = new UserData()
        {
            ad = ad,
            soyad = soyad,
            kullaniciAdi = kullaniciAdi,
            email = email,
            cinsiyet = cinsiyet,
            bolum = bolum
        };

        // Firebase'e kaydet
        RegisterUserInFirebase(userData, sifre);
    }

    private async void RegisterUserInFirebase(UserData userData, string sifre)
    {
        try
        {
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(userData.email, sifre);
            FirebaseUser newUser = authResult.User;
            Debug.LogFormat("Firebase kullanıcı kaydı başarılı: {0} ({1})", newUser.DisplayName, newUser.UserId);

            UserProfile userProfile = new UserProfile { DisplayName = userData.kullaniciAdi };
            await newUser.UpdateUserProfileAsync(userProfile);
            Debug.Log("Profil başarıyla güncellendi.");

            // E-posta doğrulama gönder
            await newUser.SendEmailVerificationAsync();
            Debug.Log("E-posta doğrulaması gönderildi: " + userData.email);

            // Kullanıcı bilgilerini Firestore'a kaydet (şifre hariç)
            DocumentReference docRef = firestore.Collection("users").Document(newUser.UserId);
            var userDictionary = new Dictionary<string, object>
            {
                { "userId", newUser.UserId }, // Unique userId
                { "ad", userData.ad },
                { "soyad", userData.soyad },
                { "kullaniciAdi", userData.kullaniciAdi },
                { "email", userData.email },
                { "cinsiyet", userData.cinsiyet },
                { "bolum", userData.bolum }
            };
            await docRef.SetAsync(userDictionary);
            Debug.Log("Kullanıcı bilgileri Firestore'a kaydedildi.");

            // Kullanıcı adını AppManager'a ata
            AppManager.Instance.userName = userData.kullaniciAdi;
        }
        catch (FirebaseException e)
        {
            Debug.LogError("Firebase kullanıcı kaydı sırasında hata oluştu: " + e.Message);
            generalErrorText.text = "Firebase kayıt hatası: " + e.Message;
            generalErrorText.gameObject.SetActive(true);
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private bool IsValidUsername(string username)
    {
        return username.Length >= 3 && username.Length <= 15;
    }

    private bool IsValidEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    private bool IsValidPassword(string password)
    {
        return password.Length >= 8 && password.Length <= 16 &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"\d") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[@$!%*?&]");
    }
}

[FirestoreData]
public class UserData
{
    [FirestoreProperty]
    public string userId { get; set; } // benzersiz User id

    [FirestoreProperty]
    public string ad { get; set; }

    [FirestoreProperty]
    public string soyad { get; set; }

    [FirestoreProperty]
    public string kullaniciAdi { get; set; }

    [FirestoreProperty]
    public string email { get; set; }

    [FirestoreProperty]
    public string cinsiyet { get; set; }

    [FirestoreProperty]
    public string bolum { get; set; }
}