using UnityEngine;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;

public class SearchListManager : MonoBehaviour
{
    public GameObject listElementPrefab; // Prefab'i buraya bırak
    public Transform content;            // Scroll View'un Content objesini buraya sürükleyin
    public TMP_InputField searchInput;   // TextMeshPro'nun TMP_InputField'ını buraya sürükleyin

    private FirebaseFirestore firestore;
    private List<string> allUsers = new List<string>();  // Kullanıcı adlarını tutacak liste
    private List<GameObject> instantiatedElements = new List<GameObject>();

    void Start()
    {
        // Firebase Firestore referansını başlat
        Debug.Log("Initializing Firebase Firestore...");
        firestore = FirebaseFirestore.DefaultInstance;

        // Firestore'dan kullanıcı adlarını yükle
        Debug.Log("Loading users from Firestore...");
        LoadUsersFromFirestore();

        // Arama alanında değişiklik olduğunda filtreleme yap
        searchInput.onValueChanged.AddListener(FilterList);
    }

    void LoadUsersFromFirestore()
    {
        firestore.Collection("users").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                QuerySnapshot snapshot = task.Result;
                Debug.Log("Successfully retrieved snapshot from Firestore.");

                // Her documentId'yi dolaşarak kullaniciAdi alanlarını çek
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    if (document.ContainsField("kullaniciAdi"))
                    {
                        string kullaniciAdi = document.GetValue<string>("kullaniciAdi");
                        allUsers.Add(kullaniciAdi);
                        Debug.Log("User found: " + kullaniciAdi);  // Her bir kullanıcı adını logla
                    }
                    else
                    {
                        Debug.LogWarning("Document does not contain 'kullaniciAdi' field: " + document.Id);
                    }
                }

                // Kullanıcı adları yüklendikten sonra listeyi güncelle
                Debug.Log($"Total users loaded: {allUsers.Count}");
                UpdateList(allUsers);
            }
            else
            {
                Debug.LogError("Failed to get user data from Firestore: " + task.Exception);
            }
        });
    }

    void UpdateList(List<string> usersToShow)
    {
        // Önce mevcut listeyi temizle
        foreach (var element in instantiatedElements)
        {
            Destroy(element);
        }
        instantiatedElements.Clear();

        // Yeni liste elemanlarını oluştur
        foreach (var user in usersToShow)
        {
            GameObject listElement = Instantiate(listElementPrefab, content);

            var textComponent = listElement.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = user;
            }

            instantiatedElements.Add(listElement);
        }
    }

    void FilterList(string searchText)
    {
        // Arama metnine göre listeyi filtrele
        List<string> filteredUsers = allUsers.FindAll(user => user.ToLower().Contains(searchText.ToLower()));

        // Filtrelenmiş listeyi güncelle
        UpdateList(filteredUsers);

        // Filtrelenmiş kullanıcı adlarını konsola yazdır
        Debug.Log("Filtered user count: " + filteredUsers.Count);
        foreach (var user in filteredUsers)
        {
            Debug.Log("Filtered user: " + user);
        }
    }
}