using UnityEngine;
using TMPro; // TextMeshPro namespace
using System.Collections.Generic;

public class SearchListManager : MonoBehaviour
{
    public GameObject listElementPrefab; // Prefab'i buraya bırak
    public Transform content;           // Scroll View'un Content objesini buraya sürükleyin
    public TMP_InputField searchInput;  // TextMeshPro'nun TMP_InputField'ını buraya sürükleyin

    private List<string> allFriends = new List<string>()
    {
        "Anakin Skywalker",
        "Elif Ozturk",
        "Anakin Amidala",
        "Obi Kenobi"
    };

    private List<GameObject> instantiatedElements = new List<GameObject>();

    void Start()
    {
        // Arama alanında değişiklik olduğunda filtreleme yap
        searchInput.onValueChanged.AddListener(FilterList);
    }

    void UpdateList(List<string> friendsToShow)
    {
        // Önce mevcut listeyi temizle
        foreach (var element in instantiatedElements)
        {
            Destroy(element);
        }
        instantiatedElements.Clear();

        // Yeni liste elemanlarını oluştur
        foreach (var friend in friendsToShow)
        {
            GameObject listElement = Instantiate(listElementPrefab, content);

            var textComponent = listElement.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = friend;
            }

            instantiatedElements.Add(listElement);
        }
    }

    void FilterList(string searchText)
    {
        // Arama metnine göre listeyi filtrele
        List<string> filteredFriends = allFriends.FindAll(friend => friend.ToLower().Contains(searchText.ToLower()));

        // Filtrelenmiş listeyi güncelle
        UpdateList(filteredFriends);
    }
}
