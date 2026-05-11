using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFactory", menuName = "Factories/Enemy Factory")]
public class EnemySpawnFactorySO : ScriptableObject
{
    [SerializeField] private EnemyAgent prefab;
    [SerializeField] private float selectionWeight = 1f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float powerAttackDamage = 30f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private bool isRare;
    [SerializeField] private float rareHealthMultiplier = 1.5f;
    [SerializeField] private float rareDamageMultiplier = 1.35f;

    public float SelectionWeight => Mathf.Max(0f, selectionWeight);
    public bool IsRare => isRare;

    public virtual EnemyAgent Spawn(Vector3 position, Quaternion rotation, Transform parent, string enemyId)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"Enemy factory '{name}' has no prefab assigned.");
            return null;
        }

        EnemyAgent enemy = Instantiate(prefab, position, rotation, parent);
        ConfigureEnemy(enemy, enemyId);
        return enemy;
    }

    protected virtual void ConfigureEnemy(EnemyAgent enemy, string enemyId)
    {
        if (enemy == null)
        {
            return;
        }

        float finalHealth = isRare ? maxHealth * rareHealthMultiplier : maxHealth;
        float finalAttackDamage = isRare ? attackDamage * rareDamageMultiplier : attackDamage;
        float finalPowerAttackDamage = isRare ? powerAttackDamage * rareDamageMultiplier : powerAttackDamage;

        enemy.ApplySpawnStats(finalAttackDamage, finalPowerAttackDamage, attackRange, stoppingDistance);

        HealthComponent health = enemy.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.SetMaxHealth(finalHealth);
        }

        EnemyId id = enemy.GetComponent<EnemyId>();
        if (id == null)
        {
            id = enemy.gameObject.AddComponent<EnemyId>();
        }

        id.Id = enemyId;

        if (AppServices.IsInitialized)
        {
            enemy.Construct(AppServices.Resolve<IAudioService>());
        }
    }
}
