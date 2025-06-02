using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Button startButton;
    public Typer typer;
    public TMP_Text timerText;
    public TMP_Text timeUpText;
    public TMP_Text scoreText;
    public TMP_Text totalScoreText;
    public WordBank wordBank;
    private int score;
    private int baseScore = 100;
    private float timeLimit = 30.0f;
    private bool gameActive = false;
    public Image startButtonBackground;
    public AudioSource timeUpSound; // Ses kaynağı

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        timerText.text = "Süre: " + timeLimit.ToString();
        timeUpText.gameObject.SetActive(false);
         baseScore = 0;
    PlayerPrefs.SetInt("TotalScore", baseScore); 
    totalScoreText.text = "Puan: " + baseScore.ToString();
        // Önceki puanı al, yoksa başlangıç puanını ayarla
        baseScore = PlayerPrefs.GetInt("TotalScore", 100);
        totalScoreText.text = "Puan: " + baseScore.ToString();
    }

    public void StartGame()
    {
        startButton.gameObject.SetActive(false);
        startButtonBackground.gameObject.SetActive(false);
        gameActive = true;
        score = 0;
        StartCoroutine(GameTimer());
        typer.SetCurrentWord();
    }

    private IEnumerator GameTimer()
    {
        float timeRemaining = timeLimit;
        
        while (timeRemaining > 0 && gameActive)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
            timerText.text = "Süre: " + timeRemaining.ToString();
        }
          if (timeRemaining <= 0)
        {
            timeUpSound.Play(); // Süre bitince ses çal
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
        scoreText.text = "Skor: " + score;
        int totalScore = baseScore + score;
        totalScoreText.text = "Puan: " + totalScore.ToString();
        
        // Toplam puanı kaydet
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.Save();

        Debug.Log("Game Over! Final Score: " + score);
    }

    private IEnumerator ShowTimeUpMessage()
    {
        Debug.Log("Time's up!");
        timeUpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        timeUpText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        startButtonBackground.gameObject.SetActive(true);
    }
}