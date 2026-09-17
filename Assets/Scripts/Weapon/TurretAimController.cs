using UnityEngine;

public class TurretAimController : MonoBehaviour
{
    [SerializeField]
    private VehicleController vehicleController;

    [Header("Rotation Settings")]
    [SerializeField]
    private float sensitivity = 0.15f;

    [SerializeField]
    private float rotationSpeed = 90f;

    [SerializeField]
    private float minAngle = -60f;

    [SerializeField]
    private float maxAngle = 60f;

    private Quaternion initialRotation;
    private float targetAngle;

    private void Awake()
    {
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        if (!vehicleController.CanBeTargeted)
            return;

        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, targetAngle, 0f);

        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void RotateHorizontal(float input)
    {
        if (!vehicleController.CanBeTargeted)
            return;

        targetAngle += input * sensitivity;
        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);
    }
}
