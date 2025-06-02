/*
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
    public Image startButtonBackground;
    private float timeLimit = 90.0f;
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
            Debug.Log("Skor: " + score);
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
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController3 : MonoBehaviour
{
    public Button startButton;
    public Typer3 typer;
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text totalScoreText; // Sağ üst köşedeki toplam puan için TMP_Text
    public WordBank3 wordBank;
    private int score;
    private int baseScore;
    private float timeLimit = 90.0f;
    private bool gameActive = false;
    public Image startButtonBackground;
    public AudioSource timeUpSound; // Ses kaynağı

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        timerText.text = "Süre: " + timeLimit.ToString();
    
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
             // Kelime sayısını hesapla
        int wordCount = typedWord.Split(new[] {' '}, System.StringSplitOptions.RemoveEmptyEntries).Length;
        score += wordCount; // Kelime sayısına göre puan ekle
        Debug.Log("Skor: " + score);
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