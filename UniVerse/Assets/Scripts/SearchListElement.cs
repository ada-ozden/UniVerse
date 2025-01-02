using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SearchListElement : MonoBehaviour
{
    public TMP_Text userNameText; // Kullanıcı Adı Text bileşeni
    public Button addButton; // Ekle Button bileşeni

    private void Start()
    {
        addButton.onClick.AddListener(OnAddButtonClicked); // Ekle Button'un OnClick event'ine dinleyici ekle
    }

    // Ekle butonuna tıklandığında çağrılacak metod
    public void OnAddButtonClicked()
    {
        // Kullanıcı adını PlayerPrefs'te kaydet
        string userName = userNameText.text;
        PlayerPrefs.SetString("FriendToAdd", userName);
        PlayerPrefs.Save();

    
    }
}