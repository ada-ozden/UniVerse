using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene Manager kütüphanesini ekliyoruz
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

    [SerializeField] private TextMeshProUGUI adErrorText;
    [SerializeField] private TextMeshProUGUI soyadErrorText;
    [SerializeField] private TextMeshProUGUI kullaniciAdiErrorText;
    [SerializeField] private TextMeshProUGUI emailErrorText;
    [SerializeField] private TextMeshProUGUI emailControlErrorText;
    [SerializeField] private TextMeshProUGUI sifreErrorText;
    [SerializeField] private TextMeshProUGUI cinsiyetErrorText;
    [SerializeField] private TextMeshProUGUI bolumErrorText;
    [SerializeField] private TextMeshProUGUI successMessageText;

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
                Debug.LogError("Firebase dependencies could not be resolved: " + dependencyStatus);
            }
        });

        if (kaydolBtn == null)
        {
            Debug.LogError("Signup button not assigned!");
        }
        else
        {
            kaydolBtn.onClick.AddListener(OnSignUpButtonClicked);
            Debug.Log("Listener added to signup button.");
        }

        ResetErrorTexts();
        successMessageText.gameObject.SetActive(false); // Hide success message initially
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firestore = FirebaseFirestore.DefaultInstance;

        // Firebase kullanıcı oturum açıldığında doğrulama durumunu kontrol et.
        auth.StateChanged += (sender, e) => {
            if (auth.CurrentUser != null)
            {
                CheckEmailVerificationStatus(auth.CurrentUser).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Error checking email verification status: " + task.Exception);
                    }
                    else if (task.IsCompleted)
                    {
                        // Email doğrulandıysa login sahnesine geçiş yap
                        bool emailVerified = task.Result;
                        if (emailVerified)
                        {
                            SceneManager.LoadScene("LoginScene"); // Login sahnesine geçiş
                        }
                    }
                });
            }
        };
    }

    public void OnSignUpButtonClicked()
    {
        if (isSubmitting) return;

        ResetErrorTexts();

        string ad = adInputField.text.Trim();
        string soyad = soyadInputField.text.Trim();
        string kullaniciAdi = kullaniciAdiInputField.text.Trim();
        string email = emailInputField.text.Trim();
        string sifre = sifreInputField.text.Trim();
        string bolum = bolumDropdown.options[bolumDropdown.value].text;
        string cinsiyet = kadinToggle.isOn ? "Kadın" : erkekToggle.isOn ? "Erkek" : "";

        bool isValid = true;

        if (string.IsNullOrWhiteSpace(ad))
        {
            isValid = false;
            adErrorText.text = "Ad alanı boş olamaz.";
            adErrorText.gameObject.SetActive(true);
        }
        if (string.IsNullOrWhiteSpace(soyad))
        {
            isValid = false;
            soyadErrorText.text = "Soyad alanı boş olamaz.";
            soyadErrorText.gameObject.SetActive(true);
        }
        if (!IsValidUsername(kullaniciAdi))
        {
            isValid = false;
            kullaniciAdiErrorText.text = "Kullanıcı adı 3-15 karakter uzunluğunda olmalı.";
            kullaniciAdiErrorText.gameObject.SetActive(true);
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            isValid = false;
            emailErrorText.text = "E-posta alanı boş olamaz.";
            emailErrorText.gameObject.SetActive(true);
        }
        else if (!IsValidEmail(email))
        {
            isValid = false;
            emailErrorText.text = "Geçerli bir e-posta adresi girin.";
            emailErrorText.gameObject.SetActive(true);
        }
        if (!IsValidPassword(sifre))
        {
            isValid = false;
            sifreErrorText.text = "Şifre 8-16 karakter arasında olmalı ve en az bir büyük harf, bir küçük harf, bir sayı ve özel karakter içermelidir.";
            sifreErrorText.gameObject.SetActive(true);
        }
        if (string.IsNullOrEmpty(cinsiyet))
        {
            isValid = false;
            cinsiyetErrorText.text = "Cinsiyet seçimi zorunludur.";
            cinsiyetErrorText.gameObject.SetActive(true);
        }
        if (bolum == "Bölümünüzü Seçiniz...")
        {
            isValid = false;
            bolumErrorText.text = "Bölüm seçiniz.";
            bolumErrorText.gameObject.SetActive(true);
        }

        if (!isValid)
        {
            isSubmitting = false;
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
            bolum = bolum,
           
        };

        // Hash the password before storing
        string hashedPassword = PasswordHasher.HashPassword(sifre);

        // Check if email exists and proceed
        CheckIfEmailExists(userData, hashedPassword);
    }

    private async void CheckIfEmailExists(UserData userData, string hashedPassword)
    {
        try
        {
            Query query = firestore.Collection("users").WhereEqualTo("email", userData.email);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Count > 0)
            {
                emailControlErrorText.text = "Bu e-posta adresi zaten var.";
                emailControlErrorText.gameObject.SetActive(true);
                isSubmitting = false;
            }
            else
            {
                await RegisterUserInFirebase(userData, hashedPassword);
            }
        }
        catch (FirebaseException e)
        {
            Debug.LogError("E-posta kontrolü sırasında hata oluştu: " + e.Message);
            emailErrorText.text = "E-posta kontrolü hatası: " + e.Message;
            emailErrorText.gameObject.SetActive(true);
            isSubmitting = false;
        }
    }

    private async Task RegisterUserInFirebase(UserData userData, string hashedPassword)
    {
        try
        {
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(userData.email, hashedPassword);
            FirebaseUser newUser = authResult.User;

            UserProfile userProfile = new UserProfile { DisplayName = userData.kullaniciAdi };
            await newUser.UpdateUserProfileAsync(userProfile);

            await newUser.SendEmailVerificationAsync();

            DocumentReference docRef = firestore.Collection("users").Document(newUser.UserId);
            var userDictionary = new Dictionary<string, object>
            {
                { "userId", newUser.UserId },
                { "ad", userData.ad },
                { "soyad", userData.soyad },
                { "kullaniciAdi", userData.kullaniciAdi },
                { "email", userData.email },
                { "cinsiyet", userData.cinsiyet },
                { "bolum", userData.bolum },
                { "hashedPassword", hashedPassword }, // Şifreyi depolama
                
            };
            await docRef.SetAsync(userDictionary);

            // MiniOyunlar Koleksiyonu
            CollectionReference miniGamesRef = docRef.Collection("MiniOyunlar");
            var miniGameDictionary = new Dictionary<string, object>
            {
                { "oyunNo", 0 },
                { "skor", 0 }
            };
            await miniGamesRef.AddAsync(miniGameDictionary);

            // Purchases Koleksiyonu
            CollectionReference purchasesRef = docRef.Collection("Purchases");
            var purchaseDictionary = new Dictionary<string, object>
            {
                { "satın_alma_no", 0 },
                { "urun_no", 0 },
                { "bakiye", 100 }
            };
            await purchasesRef.AddAsync(purchaseDictionary);

            // Ders Programı Koleksiyonu (ClassSchedule)
            CollectionReference classScheduleRef = docRef.Collection("Ders_programı");
            var classScheduleDictionary = new Dictionary<string, object>
            {
                { "dersKodu", "Bil443" },
                { "dersSaati", "09:00 " },
                { "dersGunu", "Pazartesi" }
            };
            await classScheduleRef.AddAsync(classScheduleDictionary);

            // Başarılı kayıt mesajını göster
            successMessageText.text = "Sisteme bilgileriniz kaydedildi. Emailinizi kontrol ediniz.";
            successMessageText.gameObject.SetActive(true);
        }
        catch (FirebaseException e)
        {
            Debug.LogError("Firebase kullanıcı kaydı sırasında hata oluştu: " + e.Message);
            emailErrorText.text = "Firebase kayıt hatası: " + e.Message;
            emailErrorText.gameObject.SetActive(true);
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private async Task<bool> CheckEmailVerificationStatus(FirebaseUser user)
    {
        if (user != null)
        {
            await user.ReloadAsync();
            if (user.IsEmailVerified)
            {
                Debug.Log("Email doğrulandı.");
                return true;
            }
            else
            {
                Debug.Log("Email henüz doğrulanmamış.");
            }
        }
        else
        {
            Debug.Log("Kullanıcı oturumu yok veya kullanıcı bulunamadı.");
        }
        return false;
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
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[@$!%*?&.]");
    }

    private void ResetErrorTexts()
    {
        adErrorText.text = "";
        adErrorText.gameObject.SetActive(false);

        soyadErrorText.text = "";
        soyadErrorText.gameObject.SetActive(false);

        kullaniciAdiErrorText.text = "";
        kullaniciAdiErrorText.gameObject.SetActive(false);

        emailErrorText.text = "";
        emailErrorText.gameObject.SetActive(false);

        emailControlErrorText.text = "";
        emailControlErrorText.gameObject.SetActive(false);

        sifreErrorText.text = "";
        sifreErrorText.gameObject.SetActive(false);

        cinsiyetErrorText.text = "";
        cinsiyetErrorText.gameObject.SetActive(false);

        bolumErrorText.text = "";
        bolumErrorText.gameObject.SetActive(false);

        successMessageText.text = ""; // Başarılı kayıt mesajını sıfırlama
        successMessageText.gameObject.SetActive(false); // Başarılı kayıt mesajını gizleme
    }
}

[FirestoreData]
public class UserData
{
    [FirestoreProperty]
    public string userId { get; set; }

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