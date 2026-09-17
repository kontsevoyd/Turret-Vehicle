using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
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

    private readonly List<EnemyBase> activeEnemies = new();

    private void Update()
    {
        if (target == null || !target.CanBeTargeted)
            return;

        DestroyEnemiesBehind();
    }

    public void SpawnLevelEnemies(float levelLength)
    {
        if (enemyCount <= 0)
            return;

        float availableLength = levelLength - startOffset - endOffset;
        float spacing = availableLength / enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            float z = startOffset + spacing * i + Random.Range(-randomOffsetZ, randomOffsetZ);
            float x = Random.Range(-spawnAreaWidth * 0.5f, spawnAreaWidth * 0.5f);

            Vector3 position = transform.position + new Vector3(x, 0f, z);
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            EnemyBase enemy = Instantiate(enemyPrefab, position, rotation, transform);

            enemy.SetTarget(target);
            activeEnemies.Add(enemy);
            enemy.Died.AddListener(OnEnemyDied);
        }
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

    private void OnEnemyDied(EnemyBase enemy)
    {
        enemy.Died.RemoveListener(OnEnemyDied);
        activeEnemies.Remove(enemy);
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
