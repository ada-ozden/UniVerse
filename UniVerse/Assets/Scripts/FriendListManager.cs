using UnityEngine;

public class FriendListManager : MonoBehaviour
{
    public GameObject listElementPrefab;  // Prefab'i buraya bırak
    public Transform content;            // Scroll View'un Content objesini buraya sürükleyin

    void Start()
    {
        // Örnek olarak birkaç arkadaş ekleyelim
        //while()
        for (int i = 0; i < 10; i++)
        {
            // Prefab'ı Content içine ekle
            GameObject listElement = Instantiate(listElementPrefab, content);

            // Prefab üzerindeki Text alanını güncelle
            var textComponent = listElement.GetComponentInChildren<UnityEngine.UI.Text>();
            if (textComponent != null)
            {
                textComponent.text = "Arkadaş " + (i + 1);
            }
        }
    }
}
