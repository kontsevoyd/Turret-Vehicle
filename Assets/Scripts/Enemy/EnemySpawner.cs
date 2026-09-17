using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private struct EnemySpawnData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public EnemySpawnData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }

    [SerializeField]
    private VehicleController target;

    [SerializeField]
    private EnemyBase enemyPrefab;

    [Header("Spawn Settings")]
    [SerializeField, Min(1)]
    private int enemyCount = 100;

    [SerializeField]
    private float spawnAreaWidth = 8f;

    [SerializeField]
    private float startOffset = 8f;

    [SerializeField]
    private float endOffset = 5f;

    [SerializeField]
    private float randomOffsetZ = 2f;

    [SerializeField]
    private float destroyDistance = 20f;

    private float spawnViewportY = 1f;

    private int nextSpawnIndex;

    private Plane groundPlane;

    private readonly List<EnemyBase> activeEnemies = new();
    private readonly List<EnemySpawnData> pendingEnemies = new();

    private void Awake()
    {
        groundPlane = new Plane(Vector3.up, transform.position);
    }

    private void Update()
    {
        if (target == null)
            return;

        SpawnEnemiesAhead();

        if (target.CanBeTargeted)
            DestroyEnemiesBehind();
    }

    public void SpawnLevelEnemies(float levelLength)
    {
        if (enemyCount <= 0)
            return;

        pendingEnemies.Clear();
        nextSpawnIndex = 0;

        float availableLength = levelLength - startOffset - endOffset;
        float spacing = availableLength / enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            float z = startOffset + spacing * i + Random.Range(-randomOffsetZ, randomOffsetZ);
            float x = Random.Range(-spawnAreaWidth * 0.5f, spawnAreaWidth * 0.5f);

            Vector3 position = transform.position + new Vector3(x, 0f, z);
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            pendingEnemies.Add(new EnemySpawnData(position, rotation));
        }

        pendingEnemies.Sort((a, b) => a.Position.z.CompareTo(b.Position.z));
        SpawnEnemiesAhead();
    }

    public void KillAllEnemies()
    {
        EnemyBase[] enemies = activeEnemies.ToArray();

        foreach (EnemyBase enemy in enemies)
        {
            if (enemy != null)
                enemy.Kill();
        }
    }

    private void SpawnEnemiesAhead()
    {
        float groundPositionZ = GetGroundPositionZ();

        while (nextSpawnIndex < pendingEnemies.Count)
        {
            EnemySpawnData spawnData = pendingEnemies[nextSpawnIndex];

            if (spawnData.Position.z > groundPositionZ)
                break;

            SpawnEnemy(spawnData);

            nextSpawnIndex++;
        }
    }

    private void SpawnEnemy(EnemySpawnData spawnData)
    {
        EnemyBase enemy = Instantiate(
            enemyPrefab,
            spawnData.Position,
            spawnData.Rotation,
            transform
        );

        enemy.SetTarget(target);
        activeEnemies.Add(enemy);
        enemy.Died.AddListener(OnEnemyDied);
    }

    private void OnEnemyDied(EnemyBase enemy)
    {
        enemy.Died.RemoveListener(OnEnemyDied);
        activeEnemies.Remove(enemy);
    }

    private float GetGroundPositionZ()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, spawnViewportY, 0f));

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            return point.z;
        }

        return target.transform.position.z;
    }

    private void DestroyEnemiesBehind()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            EnemyBase enemy = activeEnemies[i];

            if (enemy.transform.position.z < target.transform.position.z - destroyDistance)
            {
                activeEnemies.RemoveAt(i);
                Destroy(enemy.gameObject);
            }
        }
    }
}
