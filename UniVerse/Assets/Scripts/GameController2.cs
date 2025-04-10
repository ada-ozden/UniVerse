using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController2 : MonoBehaviour
{
    public Button startButton;
    public Typer2 typer;
    public TMP_Text timerText;  // Sayaç için text

     public TMP_Text scoreText;
    public WordBank2 wordBank;
    private int score;
    private float timeLimit = 60.0f;

    public Image startButtonBackground;
   
    private bool gameActive = false;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        timerText.text = "Süre: " + timeLimit.ToString();
    }

    public void StartGame()
    {
        startButton.gameObject.SetActive(false);
        startButtonBackground.gameObject.SetActive(false);
        gameActive = true;
        score = 0;
        StartCoroutine(GameTimer());
        typer.SetCurrentWord();  // Oyun başladığında kelimeyi ayarla
    }

    private IEnumerator GameTimer()
    {
        float timeRemaining = timeLimit;
        
        while (timeRemaining > 0 && gameActive)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
            timerText.text = "Süre: " + timeRemaining.ToString();

            // Burada isterseniz bir sayaç UI güncelleyebilirsiniz
        }

        EndGame();
    }

    public void CheckWord(string typedWord)
    {
        if (wordBank.CheckWord(typedWord))
        {
            score++;
            Debug.Log("Final Skor: " + score);
        }
    }

     private void EndGame()
    {
        gameActive = false;
        startButton.gameObject.SetActive(true);
        scoreText.text = "Skor: " + score; // Oyun sonunda skoru göster
        Debug.Log("Game Over! Final Score: " + score);
    }
     private IEnumerator ShowTimeUpMessage()
    {
        Debug.Log("Time's up!");
        
        yield return new WaitForSeconds(5f); // 5 saniye bekle
        
        startButton.gameObject.SetActive(true); // Ekrana geri döndüğünüzde butonu göster
        startButtonBackground.gameObject.SetActive(true);
    }
}