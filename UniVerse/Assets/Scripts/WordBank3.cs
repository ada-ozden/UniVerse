using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WordBank3 : MonoBehaviour
{
    private List<string> originalWords = new List<string>()
   {
    "Algoritmalar, bilgisayar biliminin yapısal taşlarıdır. ",
    "Veri yapıları, bilgiyi düzenlemenin yollarını sunar. ",
    "Mühendislik tasarım süreci, probleme çözüm bulma odaklıdır.",
    "Bilgisayar mühendisliği, algoritmaların gücünü kullanır.",
    "Yazılımlar, donanımların verimli çalışmasını sağlar.",
    "Veri yapıları, bilgiyi organize etmenin temel yoludur.",
    "Sanal bellek, fiziksel bellekten tasarruf sağlar.",
    "Protokoller, ağlar arası iletişim dilidir.",
    "İşletim sistemleri, bilgisayar donanımlarını yönetir.",
    "Mühendisliğin temeli, sistematik düşünce ve analizdir.",
    "İşletim sistemleri, cihazın yönetici yazılımıdır."
};

    private List<string> workingWords = new List<string>();

    private void Awake()
    {
        workingWords.AddRange(originalWords);
        Shuffle(workingWords);
        ConverToLower(workingWords);


    }
    private void Shuffle(List<string> list)
    {
        for(int i=0 ; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);
            string temporary = list[i];

            list[i] = list[random];
            list[random] = temporary;

        }

    }
    private void ConverToLower(List<string> list )
    {
        for(int i=0; i < list.Count ; i++)
            list[i] = list[i].ToLower();
        

    }
    public string GetWord()
    {
        string newWord = string.Empty;
        
        if(workingWords.Count != 0)
        {
            newWord = workingWords.Last();
            workingWords.Remove(newWord);
        }
        return newWord;
    }
     public bool CheckWord(string word)
    {
        return originalWords.Contains(word);
    }

   
}
