using UnityEngine;

public class BallPickupAndThrow : MonoBehaviour
{
    public Transform ballHolder;         // Assign this to the BallHolder under First Person Camera
    public float throwForce = 500f;
    public float pickupRange = 3f;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    private bool isHeld = true;          // Ball starts in hand

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // Start in hand: disable physics
        HoldBall();
    }

    void Update()
    {
        if (isHeld && Input.GetKeyDown(KeyCode.Space))
        {
            ThrowBall();
        }
        else if (!isHeld && Input.GetKeyDown(KeyCode.F))
        {
            TryPickup();
        }
    }

    void HoldBall()
    {
        isHeld = true;

        rb.isKinematic = true;
        col.enabled = false;

        transform.SetParent(ballHolder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    void ThrowBall()
    {
        isHeld = false;

        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;

        transform.localScale = Vector3.one;
        rb.velocity = Vector3.zero; // Reset existing velocity
        rb.AddForce(ballHolder.forward * throwForce);
        rb.AddTorque(Random.insideUnitSphere * 10f);
    }

    void TryPickup()
    {
        // Check if player is close enough
        float distance = Vector3.Distance(transform.position, ballHolder.position);
        if (distance <= pickupRange)
        {
            HoldBall();
        }
    }
}
