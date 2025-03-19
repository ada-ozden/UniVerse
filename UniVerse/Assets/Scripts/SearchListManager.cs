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
    private List<UserData> allUsers = new List<UserData>();  // Kullanıcı verilerini tutacak liste
    private List<GameObject> instantiatedElements = new List<GameObject>();

    private struct UserData
    {
        public string KullaniciAdi;
        public string Bolum;
    }

    void Start()
    {
        // Firebase Firestore referansını başlat
        Debug.Log("Initializing Firebase Firestore...");
        firestore = FirebaseFirestore.DefaultInstance;

        // Firestore'dan kullanıcı verilerini yükle
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

                // Her documentId'yi dolaşarak kullaniciAdi ve bolum alanlarını çek
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    if (document.ContainsField("kullaniciAdi"))
                    {
                        string kullaniciAdi = document.GetValue<string>("kullaniciAdi");
                        string bolum = document.ContainsField("bolum") ? document.GetValue<string>("bolum") : "Bölüm Bilinmiyor";

                        allUsers.Add(new UserData { KullaniciAdi = kullaniciAdi, Bolum = bolum });
                        Debug.Log($"User found: {kullaniciAdi}, Bölüm: {bolum}");
                    }
                    else
                    {
                        Debug.LogWarning("Document does not contain 'kullaniciAdi' field: " + document.Id);
                    }
                }

                // Kullanıcı verileri yüklendikten sonra listeyi güncelle
                Debug.Log($"Total users loaded: {allUsers.Count}");
                UpdateList(allUsers);
            }
            else
            {
                Debug.LogError("Failed to get user data from Firestore: " + task.Exception);
            }
        });
    }

    void UpdateList(List<UserData> usersToShow)
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
            
            // Kullanıcı adı ve bölüm bilgilerini atayın
            var textComponents = listElement.GetComponentsInChildren<TMP_Text>();
            if (textComponents.Length >= 2)
            {
                 textComponents[0].text = user.KullaniciAdi; // Kullanıcı adı
                 textComponents[1].text = user.Bolum;        // Bölüm
                 
            }
            else
            {
                Debug.LogWarning("List element doesn't contain enough TMP_Text components.");
            }

            instantiatedElements.Add(listElement);
        }
    }

    void FilterList(string searchText)
    {
        // Arama metnine göre listeyi filtrele
        List<UserData> filteredUsers = allUsers.FindAll(user => user.KullaniciAdi.ToLower().Contains(searchText.ToLower()));

        // Filtrelenmiş listeyi güncelle
        UpdateList(filteredUsers);

        // Filtrelenmiş kullanıcı adlarını konsola yazdır
        Debug.Log("Filtered user count: " + filteredUsers.Count);
        foreach (var user in filteredUsers)
        {
            Debug.Log("Filtered user: " + user.KullaniciAdi);
        }
    }
}