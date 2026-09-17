using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private LevelController levelController;
    private bool isTriggered;

    public void Initialize(LevelController levelController)
    {
        this.levelController = levelController;
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (isTriggered)
            return;

        VehicleController vehicle = collider.GetComponentInParent<VehicleController>();

        if (vehicle == null)
            return;

        isTriggered = true;

        levelController.FinishLevel();
    }
}
