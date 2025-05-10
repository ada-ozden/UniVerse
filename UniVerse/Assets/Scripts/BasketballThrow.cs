using UnityEngine;

public class BasketballThrow : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;        // Assign your player's camera here
    [SerializeField] private  Rigidbody basketballRb;        // Assign the basketball's Rigidbody
    [SerializeField] private float throwForce = 10f;

    private bool isHeld = true;

    void Update()
    {
        if (isHeld && Input.GetKeyDown(KeyCode.Space))
        {
            ThrowBall();
        }
    }

    void ThrowBall()
    {
        isHeld = false;
        basketballRb.isKinematic = false;
        basketballRb.transform.parent = null;

        // Apply force in the camera's forward direction
        basketballRb.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse);
    }
}
