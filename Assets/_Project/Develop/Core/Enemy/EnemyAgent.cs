using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyAnimationController))]
public class EnemyAgent : EnemyBase
{
    public enum EnemyType
    {
        Melee,
        Ranged
    }

    [Header("Basic settings")]
    public EnemyType type;
    [SerializeField] private float damageAmount = 15f;

    [Header("Ranged Attack")]
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform firePoint;

    [Header("References")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private HealthBarUI enemyHealthBarUI;

    private IAudioService _audioService;
    private EnemyId _enemyId;
    private bool _hasRuntimeStatsOverride;
    private RareVisualController _rareVisualController;

    public override void Construct(IAudioService audioService)
    {
        _audioService = audioService;
        base.Construct(audioService);
    }

    protected override void Awake()
    {
        base.Awake();

        if (targetTransform != null)
        {
            target = targetTransform;
        }

        if (enemyHealthBarUI != null)
        {
            healthBarUi = enemyHealthBarUI;
        }

        _enemyId = GetComponent<EnemyId>();
        _rareVisualController = GetComponent<RareVisualController>();
    }

    protected override void Start()
    {
        if (!_hasRuntimeStatsOverride)
        {
            attackDamage = damageAmount;
        }

        base.Start();
    }

    public override void ApplySpawnStats(float newAttackDamage, float newPowerAttackDamage, float newAttackRange, float newStoppingDistance)
    {
        base.ApplySpawnStats(newAttackDamage, newPowerAttackDamage, newAttackRange, newStoppingDistance);
        _hasRuntimeStatsOverride = true;
    }

    public override void ApplyRareState(bool isRare)
    {
        _rareVisualController ??= GetComponent<RareVisualController>();
        _rareVisualController?.SetRareState(isRare);
    }

    public override void PerformAttack()
    {
        ExecuteAttack(attackDamage, false);
    }

    public override void PerformPowerAttack()
    {
        ExecuteAttack(powerAttackDamage, true);
    }

    private void ExecuteAttack(float baseDamage, bool isPowerAttack)
    {
        if (isPowerAttack)
        {
            anim?.PlayPowerAttack();
        }
        else
        {
            anim?.PlayAttack();
        }

        BossElementController bossElement = GetComponent<BossElementController>();
        float physicalDamage = type == EnemyType.Melee ? baseDamage : 0f;
        float magicDamage = type == EnemyType.Ranged ? baseDamage : 0f;

        if (type == EnemyType.Melee)
        {
            bossElement?.ModifyDamage(isPowerAttack, ref physicalDamage, ref magicDamage);
        }

        if (type == EnemyType.Melee)
        {
            bossElement?.PlayWeaponAttackEffect();
            _audioService?.PlaySound("Enemy_Attack_Melee");

            if (CanDamageTarget(GetFlatDistanceToTarget()))
            {
                targetDamageable.TakeDamage(physicalDamage, magicDamage);
                _audioService?.PlaySound("Player_Hit");
            }

            return;
        }

        _audioService?.PlaySound("Enemy_Attack_Ranged");
        if (magicPrefab == null || firePoint == null || target == null)
        {
            return;
        }

        Vector3 aimDirection = (target.position + Vector3.up) - firePoint.position;
        GameObject fireball = Object.Instantiate(magicPrefab, firePoint.position, Quaternion.LookRotation(aimDirection));

        if (fireball.TryGetComponent<MagicProjectile>(out var projectile))
        {
            projectile.Setup(physicalDamage, magicDamage, false);
        }
    }
}
