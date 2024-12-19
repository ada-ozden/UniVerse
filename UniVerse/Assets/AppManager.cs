using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }
    
    public string userName;

    private void Awake()
    {
        // Eğer başka bir instance varsa mevcut instance'ı yok et
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // Oyun sahneleri değişirken objeyi yok etme
    }
}