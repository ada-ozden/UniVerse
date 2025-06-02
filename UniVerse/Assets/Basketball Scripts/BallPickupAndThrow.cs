using UnityEngine;
using UnityEngine.UI;

public class BallPickupAndThrow : MonoBehaviour
{
    public Transform ballHolder;
    public float minThrowForce = 300f;
    public float maxThrowForce = 1000f;
    public float chargeSpeed = 500f;
    public Slider powerSlider;

    public BallTrajectory trajectoryVisualizer;

    private float currentThrowForce;
    private bool isCharging = false;
    private bool isHeld = true;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;

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

                if (powerSlider != null)
                {
                    powerSlider.value = currentThrowForce;
                    powerSlider.gameObject.SetActive(true);
                }
            }

            if (isCharging)
            {
                currentThrowForce += chargeSpeed * Time.deltaTime;
                currentThrowForce = Mathf.Clamp(currentThrowForce, minThrowForce, maxThrowForce);

                if (powerSlider != null)
                    powerSlider.value = currentThrowForce;

                if (trajectoryVisualizer != null)
                {
                    Vector3 force = ballHolder.forward * currentThrowForce;
                    trajectoryVisualizer.ShowTrajectory(rb, force);
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isCharging = false;

                if (powerSlider != null)
                    powerSlider.gameObject.SetActive(false);

                if (trajectoryVisualizer != null)
                    trajectoryVisualizer.HideTrajectory();

                ThrowBall(currentThrowForce);
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

    void ThrowBall(float force)
    {
        isHeld = false;

        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;

        rb.velocity = Vector3.zero;
        rb.AddForce(ballHolder.forward * force);
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
