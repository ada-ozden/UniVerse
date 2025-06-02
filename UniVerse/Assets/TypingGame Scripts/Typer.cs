using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class Typer : MonoBehaviour
{
    public WordBank wordBank = null;
    public TMP_Text wordOutput = null;
    public string remainingWord = string.Empty;
    public string currentWord = string.Empty;
    

    
    public void Start()
    {
        SetCurrentWord();
        
    }

    public void SetCurrentWord()
    {
        currentWord = wordBank.GetWord();
        SetRemainingWord(currentWord);

    }
    public void SetRemainingWord(string newString)
    {
        remainingWord = newString;
        wordOutput.text = remainingWord;

    }

   
    public void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {
        if(Input.anyKeyDown)
        {
            string keysPressed = Input.inputString;
            
            if (keysPressed.Length == 1)
                EnterLetter(keysPressed);

        }
    }

public void EnterLetter(string typedLetter)
{
    if(IsCorrectLetter(typedLetter))
    {
        RemoveLetter();
        if (IsWordComplete())
        {
             FindObjectOfType<GameController>().CheckWord(currentWord);
            SetCurrentWord();
        }
    }
}

    public bool IsCorrectLetter(string letter)
    {
        return remainingWord.IndexOf(letter) == 0;
    }

    public void RemoveLetter()
    {
        string newString = remainingWord.Remove(0,1);
        SetRemainingWord(newString);
    }

    public bool IsWordComplete()
    {
        return remainingWord.Length == 0;
    }

}
