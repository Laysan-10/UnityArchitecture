using System;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private string bossId = "boss";
    [SerializeField] private bool spawnOnAwake = true;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform container;
    [SerializeField] private BossSpawnFactorySO[] bossFactories;

    private void Awake()
    {
        EnsureBossId();

        if (spawnOnAwake)
        {
            SpawnBoss();
        }
    }

    [ContextMenu("Spawn Boss")]
    public EnemyAgent SpawnBoss()
    {
        if (bossFactories == null || bossFactories.Length == 0)
        {
            Debug.LogWarning($"Boss spawner '{name}' has no boss factories configured.", this);
            return null;
        }

        BossSpawnFactorySO factory = bossFactories[UnityEngine.Random.Range(0, bossFactories.Length)];
        BossElementController.BossElementType element =
            (BossElementController.BossElementType)UnityEngine.Random.Range(
                0,
                Enum.GetValues(typeof(BossElementController.BossElementType)).Length);

        Transform point = spawnPoint != null ? spawnPoint : transform;
        return factory != null
            ? factory.SpawnBoss(point.position, point.rotation, container, bossId, element)
            : null;
    }

    private void OnValidate()
    {
        EnsureBossId();
    }

    private void EnsureBossId()
    {
        if (string.IsNullOrWhiteSpace(bossId))
        {
            bossId = Guid.NewGuid().ToString("N");
        }
    }
}
