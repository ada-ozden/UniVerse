using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public string fileName = "ders_program_cıktı.csv";
    public List<string[]> data = new List<string[]>();

    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            data.Add(line.Split(',')); // Hücrelere ayır
        }
    }
}
