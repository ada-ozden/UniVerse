using UnityEngine;
using UnityEngine.UI;  // UI sınıflarını kullanmak için gerekli
using TMPro;          // TextMeshPro sınıflarını kullanmak için gerekli
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

    private string baseUrl = "http://localhost:3333/user/register"; // Backend URL'iniz
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
        if (isSubmitting) return; // Halen bir istek gönderiliyorsa işlemi durdur
        isSubmitting = true;      // Yeni bir istek gönderiliyor

        Debug.Log("Button pressed");
        string ad = adInputField.text;
        string soyad = soyadInputField.text;
        string kullaniciAdi = kullaniciAdiInputField.text;
        string email = emailInputField.text;
        string sifre = sifreInputField.text;
        string cinsiyet = kadinToggle.isOn ? "Kadın" : "Erkek";
        string bolum = bolumDropdown.options[bolumDropdown.value].text;

        Debug.Log($"Kullanıcı Kaydı: {ad}, {soyad}, {kullaniciAdi}, {email}, {sifre}, {cinsiyet}, {bolum}");

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

        // JSON data gönderimi
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

        isSubmitting = false; // İstek tamamlandı

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Hata: {request.error}");
        }
        else
        {
            Debug.Log($"Kullanıcı başarıyla kaydedildi: {request.downloadHandler.text}");
        }
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