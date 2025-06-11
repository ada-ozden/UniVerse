using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public string fileName = "ders_program_cıktı.csv";  // StreamingAssets/schedule.csv
    public GameObject rowPrefab;              // Row prefab’ı
    public Transform contentParent;           // ScrollView > Viewport > Content

    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        bool exists = File.Exists(path);

        if (!exists)
        {
            Debug.LogError("CSV dosyası bulunamadı: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);
        Debug.Log($"[CSVReader] Found {lines.Length} lines—will spawn {lines.Length} rows.");

        foreach (string line in lines)
        {
            string[] cells = line.Split(',');

            GameObject row = Instantiate(rowPrefab, contentParent);
            TextMeshProUGUI[] textCells = row.GetComponentsInChildren<TextMeshProUGUI>();

            for (int i = 0; i < cells.Length && i < textCells.Length; i++)
            {
                textCells[i].text = cells[i].Replace("\r\n", "\n").Replace("\r", "\n");
            }
        }
    }
}
