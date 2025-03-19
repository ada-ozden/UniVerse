using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MessageFilter : MonoBehaviour
{
    private List<string> bannedWords = new List<string>();

    void Start()
    {
        LoadBannedWords();
    }

    private void LoadBannedWords()
    {
        // JSON dosyasını okuyun
        TextAsset jsonTextFile = Resources.Load<TextAsset>("karaliste");
        
        if (jsonTextFile != null)
        {
            bannedWords = JsonUtility.FromJson<ListWrapper>(jsonTextFile.text).words;
        }
        else
        {
            Debug.LogError("Yasaklı kelime dosyası bulunamadı.");
        }
    }

    public string FilterMessage(string message)
    {
        if (bannedWords == null || bannedWords.Count == 0)
            return message;

        foreach (string word in bannedWords)
        {
            if (message.Contains(word))
            {
                string replacement = new string('*', word.Length);
                message = message.Replace(word, replacement);
            }
        }
        return message;
    }
}

[System.Serializable]
public class ListWrapper
{
    public List<string> words;
}