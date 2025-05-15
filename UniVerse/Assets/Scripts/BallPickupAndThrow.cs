using UnityEngine;
using UnityEngine.UI; // Slider için

public class BallPickupAndThrow : MonoBehaviour
{
    public Transform ballHolder;
    public float minThrowForce = 300f;
    public float maxThrowForce = 1000f;
    public float chargeSpeed = 500f;
    public Slider powerSlider; // Sağ alttaki bar

    private float currentThrowForce;
    private bool isCharging = false;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    private bool isHeld = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        HoldBall();

        if (powerSlider != null)
        {
            powerSlider.minValue = minThrowForce;
            powerSlider.maxValue = maxThrowForce;
            powerSlider.value = minThrowForce;
            powerSlider.gameObject.SetActive(false); // Başta gizli
        }
    }

    void Update()
    {
        if (isHeld)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isCharging = true;
                currentThrowForce = minThrowForce;
                powerSlider.gameObject.SetActive(true);
            }

            if (isCharging)
            {
                currentThrowForce += chargeSpeed * Time.deltaTime;
                currentThrowForce = Mathf.Clamp(currentThrowForce, minThrowForce, maxThrowForce);
                powerSlider.value = currentThrowForce;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isCharging = false;
                ThrowBall();
                powerSlider.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
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

        rb.velocity = Vector3.zero;
        rb.AddForce(ballHolder.forward * currentThrowForce);
        rb.AddTorque(Random.insideUnitSphere * 10f);
    }

    void TryPickup()
    {
        float distance = Vector3.Distance(transform.position, ballHolder.position);
        if (distance <= 3f)
        {
            HoldBall();
        }
    }
}
