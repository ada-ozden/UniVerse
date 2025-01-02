using UnityEngine;
using TMPro;

public class FriendListManager : MonoBehaviour
{
    public GameObject listElementPrefab; // Prefab'i buraya bırak
    public Transform content;            // Scroll View'un Content objesini buraya sürükleyin

    void Start()
    {
        // PlayerPrefs'ten arkadaş eklemeyi al
        if (PlayerPrefs.HasKey("FriendToAdd"))
        {
            string friendName = PlayerPrefs.GetString("FriendToAdd");
            AddFriendToList(friendName);

            // Eklenmiş arkadaş adını temizle
            PlayerPrefs.DeleteKey("FriendToAdd");
            PlayerPrefs.Save();
        }
    }

    void AddFriendToList(string friendName)
    {
        // Yeni liste elemanını oluştur
        GameObject listElement = Instantiate(listElementPrefab, content);

        var textComponent = listElement.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = friendName;
        }
    }
}