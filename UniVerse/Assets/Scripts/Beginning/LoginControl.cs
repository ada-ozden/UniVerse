using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class LoginForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField sifreInputField;
    [SerializeField] private Button loginBtn;

    private string loginUrl = "http://localhost:3333/user/login"; // Backend URL'iniz

    void Start()
    {
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

    public void OnLoginButtonClicked()
    {
        Debug.Log("Giriş Yap button pressed");
        string email = emailInputField.text;
        string sifre = sifreInputField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
        {
            Debug.LogWarning("Email or Sifre should not be empty.");
            return;
        }

        var loginData = new LoginData()
        {
            email = email,
            sifre = sifre
        };

        string jsonData = JsonUtility.ToJson(loginData);
        Debug.Log($"Gönderilen JSON Verisi: {jsonData}");
        StartCoroutine(LoginUser(jsonData));
    }

    private IEnumerator LoginUser(string jsonData)
    {
        var request = new UnityWebRequest(loginUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Hata: {request.error}");
        }
        else
        {
            Debug.Log($"Response Code: {request.responseCode}");
            Debug.Log($"Backend Yanıtı: {request.downloadHandler.text}");

            // Yanıt koduna göre sonucu belirleyelim
            if (request.responseCode == 200 || request.responseCode == 201)
            {
                Debug.Log("Giriş başarılı: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Giriş başarısız: " + request.downloadHandler.text);
            }
        }
    }
}

[System.Serializable]
public class LoginData
{
    public string email;
    public string sifre;
}