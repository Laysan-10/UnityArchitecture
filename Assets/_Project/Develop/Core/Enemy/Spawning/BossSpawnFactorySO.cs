using UnityEngine;

[CreateAssetMenu(fileName = "BossFactory", menuName = "Factories/Boss Factory")]
public class BossSpawnFactorySO : ScriptableObject
{
    [SerializeField] private EnemyBase prefab;
    [SerializeField] private float maxHealth = 150f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float powerAttackDamage = 30f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float stoppingDistance = 6f;
    [SerializeField] private bool isRare;
    [SerializeField] private float rareHealthMultiplier = 1.5f;
    [SerializeField] private float rareDamageMultiplier = 1.35f;

    public EnemyBase SpawnBoss(
        Vector3 position,
        Quaternion rotation,
        Transform parent,
        string enemyId,
        BossElementController.BossElementType element)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"Boss factory '{name}' has no prefab assigned.");
            return null;
        }

        EnemyBase enemy = Instantiate(prefab, position, rotation, parent);
        if (enemy == null)
        {
            return null;
        }

        float finalHealth = isRare ? maxHealth * rareHealthMultiplier : maxHealth;
        float finalAttackDamage = isRare ? attackDamage * rareDamageMultiplier : attackDamage;
        float finalPowerAttackDamage = isRare ? powerAttackDamage * rareDamageMultiplier : powerAttackDamage;

        enemy.ApplySpawnStats(finalAttackDamage, finalPowerAttackDamage, attackRange, stoppingDistance);
        enemy.ApplyRareState(isRare);

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

        BossElementController elementController =
            enemy.GetComponent<BossElementController>() ??
            enemy.gameObject.AddComponent<BossElementController>();

        elementController.Initialize(element, BossElementController.BossWeaponMode.Melee);
        return enemy;
    }
}
