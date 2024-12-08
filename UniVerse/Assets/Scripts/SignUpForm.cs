using UnityEngine;
using UnityEngine.UI;
using TMPro;  // For TextMeshPro elements
using UnityEngine.Networking;
using System.Collections;
using System.Text;

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

    // Single UI element for general error message
    [SerializeField] private TextMeshProUGUI generalErrorText;

    private string baseUrl = "http://localhost:3333/user/register";
    private bool isSubmitting = false;

    void Start()
    {
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

    public void OnSignUpButtonClicked()
    {
        if (isSubmitting) return;

        // Clear previous errors
        generalErrorText.text = "";
        generalErrorText.gameObject.SetActive(false);

        string ad = adInputField.text.Trim();
        string soyad = soyadInputField.text.Trim();
        string kullaniciAdi = kullaniciAdiInputField.text.Trim();
        string email = emailInputField.text.Trim();
        string sifre = sifreInputField.text.Trim();
        string cinsiyet = kadinToggle.isOn ? "Kadın" : erkekToggle.isOn ? "Erkek" : "";
        string bolum = bolumDropdown.options[bolumDropdown.value].text;

        // Validate inputs
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
            isSubmitting = false;
            return;
        }

        // If valid, proceed with the request
        isSubmitting = true;

        var userData = new UserData()
        {
            ad = ad,
            soyad = soyad,
            kullaniciAdi = kullaniciAdi,
            email = email,
            sifre = sifre,
            cinsiyet = cinsiyet,
            bolum = bolum
        };

        string jsonData = JsonUtility.ToJson(userData);
        Debug.Log($"Gönderilen JSON Verisi: {jsonData}");
        StartCoroutine(RegisterUser(jsonData));
    }

    private IEnumerator RegisterUser(string jsonData)
    {
        var request = new UnityWebRequest(baseUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        isSubmitting = false;

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Hata: {request.error}");
        }
        else
        {
            Debug.Log($"Kullanıcı başarıyla kaydedildi: {request.downloadHandler.text}");
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

[System.Serializable]
public class UserData
{
    public string ad;
    public string soyad;
    public string kullaniciAdi;
    public string email;
    public string sifre;
    public string cinsiyet;
    public string bolum;
}
