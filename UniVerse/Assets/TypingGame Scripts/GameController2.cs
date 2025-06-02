using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController2 : MonoBehaviour
{
    public Button startButton;
    public Typer2 typer;
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text totalScoreText; // Sağ üstteki toplam puan için TMP_Text
    public WordBank2 wordBank;
    private int score;
    private int baseScore;
    private float timeLimit = 60.0f;
    public Image startButtonBackground;
    private bool gameActive = false;
    public AudioSource timeUpSound; // Ses kaynağı

    void Start()
    {
  
        startButton.onClick.AddListener(StartGame);
        timerText.text = "Süre: " + timeLimit.ToString();
        // Önceki puanı al, yoksa başlangıç puanını ayarla
        baseScore = PlayerPrefs.GetInt("TotalScore", 100);
        totalScoreText.text = "UniCoin: " + baseScore.ToString();
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
            Debug.Log("Final Skor: " + score);
        }
    }

    private void EndGame()
    {
        gameActive = false;
        startButton.gameObject.SetActive(true);
        int totalScore = baseScore + score;
        scoreText.text = "Skor: " + score;
        totalScoreText.text = "Puan: " + totalScore.ToString(); // Toplam puanı güncelle

        // Toplam puanı kaydet
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.Save();

        Debug.Log("Game Over! Final Score: " + score);
    }

    private IEnumerator ShowTimeUpMessage()
    {
        Debug.Log("Time's up!");
        yield return new WaitForSeconds(5f);
        startButton.gameObject.SetActive(true);
        startButtonBackground.gameObject.SetActive(true);
    }
}