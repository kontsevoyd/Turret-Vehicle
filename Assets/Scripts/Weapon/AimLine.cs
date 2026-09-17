using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimLine : MonoBehaviour
{
    [SerializeField]
    private Transform muzzlePoint;

    [SerializeField, Min(0f)]
    private float lineLength = 10f;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        Vector3 start = muzzlePoint.position;
        Vector3 end = start + muzzlePoint.forward * lineLength;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
