using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BallTrajectory : MonoBehaviour
{
    public int resolution = 30;
    public float timeStep = 0.1f;
    public LayerMask hitMask;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    /// <summary>
    /// Trajektoriyi gösterir.
    /// </summary>
    /// <param name="rb">Topun Rigidbody'si</param>
    /// <param name="force">Atılacak kuvvet vektörü</param>
    public void ShowTrajectory(Rigidbody rb, Vector3 force)
    {
        lineRenderer.enabled = true;
        Vector3[] points = new Vector3[resolution];

        Vector3 startPosition = rb.position;
        Vector3 startVelocity = force / rb.mass;

        for (int i = 0; i < resolution; i++)
        {
            float time = i * timeStep;
            Vector3 point = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;
            points[i] = point;

            if (i > 0 && Physics.Linecast(points[i - 1], point, out RaycastHit hit, hitMask))
            {
                lineRenderer.positionCount = i;
                lineRenderer.SetPositions(points);
                return;
            }
        }

        lineRenderer.positionCount = resolution;
        lineRenderer.SetPositions(points);
    }

    public void HideTrajectory()
    {
        lineRenderer.enabled = false;
    }
}
