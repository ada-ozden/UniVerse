using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SFB; // StandaloneFileBrowser için

public class FileUploadManager : MonoBehaviour
{
    public Button uploadButton; // Inspector'dan butonu bağlayacağız

    void Start()
    {
        // Butonun tıklama olayına fonksiyonu ekle
        if (uploadButton != null)
        {
            uploadButton.onClick.AddListener(OpenFileSelectionDialog);
        }
        else
        {
            Debug.LogError("Upload Button not assigned in Inspector!");
        }
    }

    // Dosya Seçme Diyaloğunu Açan Fonksiyon
    void OpenFileSelectionDialog()
    {
        // Tek dosya seçimi için StandaloneFileBrowser.OpenFilePanelAsync kullanılır.
        // Parametreler: Başlık, varsayılan dizin, filtreler (uzantılar), çoklu seçim
        // Filtreler: new ExtensionFilter("Resim Dosyaları", "png", "jpg"), new ExtensionFilter("Metin Dosyaları", "txt", "pdf")
        StandaloneFileBrowser.OpenFilePanelAsync("Dosya Seç", "", "", false, (string[] paths) =>
        {
            // Seçilen dosya yolları "paths" dizisinde döner.
            if (paths.Length > 0)
            {
                string selectedFilePath = paths[0]; // Sadece ilk seçilen dosyayı alıyoruz
                Debug.Log("Seçilen Dosya Yolu: " + selectedFilePath);

                // Seçilen dosyayı StreamingAssets klasörüne kopyala
                CopyFileToStreamingAssets(selectedFilePath);
            }
        });
    }

    // Dosyayı StreamingAssets'e kopyalayan fonksiyon
    void CopyFileToStreamingAssets(string sourceFilePath)
    {
        // StreamingAssets klasörünün yolu
        string streamingAssetsPath = Application.streamingAssetsPath;

        // Klasör yoksa oluştur
        if (!Directory.Exists(streamingAssetsPath))
        {
            Directory.CreateDirectory(streamingAssetsPath);
        }

        // Hedef dosya yolu (StreamingAssets klasöründe, orijinal dosya adıyla)
        string fileName = Path.GetFileName(sourceFilePath);
        string destinationFilePath = Path.Combine(streamingAssetsPath, fileName);

        try
        {
            File.Copy(sourceFilePath, destinationFilePath, true); // true: Hedefte dosya varsa üzerine yaz
            Debug.Log($"Dosya başarıyla kopyalandı: {destinationFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Dosya kopyalama hatası: {e.Message}");
        }
    }
}