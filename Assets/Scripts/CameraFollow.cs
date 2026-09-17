using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    public float cameraOffsetZ = -15;

    void LateUpdate()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            cameraOffsetZ + target.position.z
        );
    }
}
