using UnityEngine;
using TMPro; // Eğer TextMeshPro kullanıyorsan

public class BasketScoreManager : MonoBehaviour
{
    public Transform ball;
    public Transform player;
    public Collider twoPointZone;

    public TMP_Text scoreText; // UI Text nesnesini buraya sürükle

    private int score = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == ball)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();

            if (ballRb.velocity.y < 0)
            {
                bool isInTwoPointZone = twoPointZone.bounds.Contains(player.position);

                if (isInTwoPointZone)
                {
                    score += 2;
                    Debug.Log("2 puan! Toplam puan: " + score);
                }
                else
                {
                    score += 3;
                    Debug.Log("3 puan! Toplam puan: " + score);
                }

                UpdateScoreText(); // UI'yı güncelle
            }
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}

