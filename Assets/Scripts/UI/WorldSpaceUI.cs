using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = mainCamera.transform.rotation;
    }
}
