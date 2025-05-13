using UnityEngine;

public class BasketScoreManager : MonoBehaviour
{
    public Transform ball;             // Assign the Basketball object
    public Transform player;           // Assign the player object
    public Collider twoPointZone;      // Assign the 2-point zone collider

    private int score = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == ball)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            
            // Only score if ball is moving downward
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
            }
        }
    }
}
