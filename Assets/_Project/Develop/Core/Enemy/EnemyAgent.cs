using System.Collections.Generic;
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

    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;

        if (health == null)
        {
            health = GetComponent<HealthComponent>();
        }

        health?.Construct(audioService);
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
    }

    protected override void Start()
    {
        attackDamage = damageAmount;
        base.Start();
    }

    public override void PerformAttack()
    {
        anim?.PlayAttack();

        if (type == EnemyType.Melee)
        {
            _audioService?.PlaySound("Enemy_Attack_Melee");

            if (CanDamageTarget(GetFlatDistanceToTarget()))
            {
                targetDamageable.TakeDamage(attackDamage, 0f);
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
            projectile.Setup(attackDamage, false);
        }
    }

    public EnemySaveData CaptureState()
    {
        if (_enemyId == null)
        {
            _enemyId = GetComponent<EnemyId>();
        }

        if (health == null)
        {
            health = GetComponent<HealthComponent>();
        }

        if (_enemyId == null || health == null)
        {
            return null;
        }

        return new EnemySaveData
        {
            Id = _enemyId.Id,
            Position = transform.position,
            CurrentHp = health.Core.CurrentHealth,
            IsDead = health.Core.IsDead
        };
    }

    public void RestoreState(IReadOnlyList<EnemySaveData> enemyStates)
    {
        if (_enemyId == null)
        {
            _enemyId = GetComponent<EnemyId>();
        }

        if (health == null)
        {
            health = GetComponent<HealthComponent>();
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (anim == null)
        {
            anim = GetComponent<EnemyAnimationBase>();
        }

        if (_enemyId == null || health == null || agent == null || anim == null)
        {
            return;
        }

        EnemySaveData myData = null;
        foreach (EnemySaveData enemyState in enemyStates)
        {
            if (enemyState.Id == _enemyId.Id)
            {
                myData = enemyState;
                break;
            }
        }

        if (myData == null)
        {
            return;
        }

        if (myData.IsDead)
        {
            health.Core.RestoreHealth(0);
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (healthBarUi != null)
        {
            healthBarUi.gameObject.SetActive(true);
        }

        health.Core.RestoreHealth(myData.CurrentHp);

        agent.enabled = false;
        transform.position = myData.Position;
        agent.enabled = true;
        agent.isStopped = false;

        anim.ResetVisuals();
        ResetBehaviorState();
        StateMachine = new EnemyStateMachine();
        StateMachine.ChangeState(CreateState(EnemyStateType.Idle));
    }
}
