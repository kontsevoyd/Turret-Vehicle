using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    EnemySpawner enemySpawner;

    [SerializeField]
    VehicleController vehicleController;

    [SerializeField]
    private FinishTrigger finishTrigger;

    [SerializeField]
    private float levelLength = 250f;

    [SerializeField]
    private float afterFinishDriveDistance = 10f;

    public float LevelLength => levelLength;
    public UnityEvent Finished = new();

    private bool isFinishing;

    private void Start()
    {
        GenerateFinish();
        enemySpawner.SpawnLevelEnemies(levelLength);
    }

    public void StartLevel()
    {
        vehicleController.StartVehicle();
    }

    public void StopLevel()
    {
        vehicleController.StopVehicle();
    }

    public void FinishLevel()
    {
        if (isFinishing)
            return;

        StartCoroutine(FinishSequence());
    }

    private void GenerateFinish()
    {
        finishTrigger.transform.position = transform.position + Vector3.forward * levelLength;
        finishTrigger.Initialize(this);
    }

    private IEnumerator FinishSequence()
    {
        isFinishing = true;

        vehicleController.DisableCombat();
        enemySpawner.KillAllEnemies();

        float stopPositionZ = vehicleController.transform.position.z + afterFinishDriveDistance;

        while (vehicleController.transform.position.z < stopPositionZ)
        {
            yield return null;
        }

        Finished.Invoke();
    }
}
