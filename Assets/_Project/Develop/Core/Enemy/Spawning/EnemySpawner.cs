using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private string spawnerId;
    [SerializeField] private bool spawnOnAwake = true;
    [SerializeField] private int spawnCount = 3;
    [SerializeField] private Transform container;

    [Header("Factories")]
    [SerializeField] private EnemySpawnFactorySO[] factories;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private bool allowPointReuse = true;

    private readonly List<EnemyAgent> _spawnedEnemies = new List<EnemyAgent>();

    private void Awake()
    {
        EnsureSpawnerId();

        if (spawnOnAwake)
        {
            SpawnAll();
        }
    }

    [ContextMenu("Spawn All")]
    public void SpawnAll()
    {
        if (factories == null || factories.Length == 0)
        {
            Debug.LogWarning($"Spawner '{name}' has no enemy factories configured.", this);
            return;
        }

        _spawnedEnemies.Clear();
        List<Transform> availablePoints = BuildPointPool();
        int count = Mathf.Max(1, spawnCount);

        for (int i = 0; i < count; i++)
        {
            Transform point = PickSpawnPoint(availablePoints);
            EnemySpawnFactorySO factory = PickFactory();
            if (factory == null || point == null)
            {
                continue;
            }

            string enemyId = $"{spawnerId}_{i}";
            EnemyAgent enemy = factory.Spawn(point.position, point.rotation, container, enemyId);
            if (enemy != null)
            {
                _spawnedEnemies.Add(enemy);
            }
        }
    }

    private List<Transform> BuildPointPool()
    {
        List<Transform> result = new List<Transform>();

        if (spawnPoints != null)
        {
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                {
                    result.Add(point);
                }
            }
        }

        if (result.Count == 0)
        {
            result.Add(transform);
        }

        return result;
    }

    private Transform PickSpawnPoint(List<Transform> availablePoints)
    {
        if (availablePoints == null || availablePoints.Count == 0)
        {
            return transform;
        }

        int index = UnityEngine.Random.Range(0, availablePoints.Count);
        Transform point = availablePoints[index];

        if (!allowPointReuse && availablePoints.Count > 1)
        {
            availablePoints.RemoveAt(index);
        }

        return point;
    }

    private EnemySpawnFactorySO PickFactory()
    {
        float totalWeight = 0f;
        foreach (EnemySpawnFactorySO factory in factories)
        {
            if (factory != null)
            {
                totalWeight += factory.SelectionWeight;
            }
        }

        if (totalWeight <= 0f)
        {
            return factories[UnityEngine.Random.Range(0, factories.Length)];
        }

        float roll = UnityEngine.Random.value * totalWeight;
        float accumulated = 0f;

        foreach (EnemySpawnFactorySO factory in factories)
        {
            if (factory == null)
            {
                continue;
            }

            accumulated += factory.SelectionWeight;
            if (roll <= accumulated)
            {
                return factory;
            }
        }

        return factories[0];
    }

    private void OnValidate()
    {
        EnsureSpawnerId();
    }

    private void EnsureSpawnerId()
    {
        if (string.IsNullOrWhiteSpace(spawnerId))
        {
            spawnerId = Guid.NewGuid().ToString("N");
        }
    }
}
