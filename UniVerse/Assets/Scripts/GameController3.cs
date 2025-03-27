using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController3 : MonoBehaviour
{
    public Button startButton;
    public Typer3 typer;
    public TMP_Text timerText;  // Sayaç için text

     public TMP_Text scoreText;
    public WordBank3 wordBank;
    private int score;
    private float timeLimit = 90.0f;
    private bool gameActive = false;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        timerText.text = "Time: " + timeLimit.ToString();
    }

    public void StartGame()
    {
        startButton.gameObject.SetActive(false);
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
            timerText.text = "Time: " + timeRemaining.ToString();

            // Burada isterseniz bir sayaç UI güncelleyebilirsiniz
        }

        EndGame();
    }

    public void CheckWord(string typedWord)
    {
        if (wordBank.CheckWord(typedWord))
        {
            score++;
            Debug.Log("Score: " + score);
        }
    }

     private void EndGame()
    {
        gameActive = false;
        startButton.gameObject.SetActive(true);
        scoreText.text = "Final Score: " + score; // Oyun sonunda skoru göster
        Debug.Log("Game Over! Final Score: " + score);
    }
}