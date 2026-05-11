using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HealthComponent))]
public class EnemyBase : MonoBehaviour
{
    public EnemyStateMachine StateMachine;

    [Header("Settings")]
    public float lookRadius = 15f;
    public float attackRange = 2.5f;
    public float attackCooldown = 1.5f;
    public float stoppingDistance = 2f;
    public float attackDamage = 15f;
    public float powerAttackDamage = 30f;
    public float fleeHealthThreshold = 0.25f;
    public bool isPeaceful = false;
    public bool isBoss = false;
    public bool canFlee = true;
    public bool allowRepeatFlee = false;

    [Header("References")]
    public NavMeshAgent agent;
    public EnemyAnimationBase anim;
    public HealthComponent health;
    public Transform target;
    public HealthBarUI healthBarUi;
    public IDamageable targetDamageable;

    public bool IsProvoked { get; private set; }
    public bool HasRetreated { get; private set; }
    public bool HasEnteredEnragedState { get; private set; }
    public Vector3 TrackedTargetPosition { get; private set; }

    private EnemyAttackSelector _attackSelector;
    private EnemyId _enemyId;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<EnemyAnimationBase>();
        health = GetComponent<HealthComponent>();
        healthBarUi = GetComponentInChildren<HealthBarUI>(true);
        _attackSelector = new EnemyAttackSelector(this);
        _enemyId = GetComponent<EnemyId>();
    }

    protected virtual void Start()
    {
        StateMachine = new EnemyStateMachine();
        ResolveTargetReferences();

        if (anim != null && health != null)
        {
            anim.Construct(this, health.Core);
        }

        if (healthBarUi != null && health != null)
        {
            healthBarUi.Construct(health.Core);
        }

        if (agent != null)
        {
            agent.stoppingDistance = stoppingDistance;
        }

        if (health != null)
        {
            health.Core.OnDamaged += HandleDamaged;
        }

        ResetBehaviorState();
        StateMachine.ChangeState(new IdleState(this));
    }

    protected virtual void Update()
    {
        if (health != null && health.Core.IsDead)
        {
            if (!StateMachine.IsInState<DeadState>())
            {
                StateMachine.ChangeState(new DeadState(this));
            }

            return;
        }

        StateMachine.Update();
        RememberTargetPosition();
    }

    protected virtual void OnDestroy()
    {
        if (health != null)
        {
            health.Core.OnDamaged -= HandleDamaged;
        }
    }

    public float GetAttackSpeed()
    {
        if (isBoss && health.Core.CurrentHealth < health.Core.MaxHealth * 0.5f)
        {
            return 0.5f;
        }

        return attackCooldown;
    }

    public virtual bool ShouldRetreat()
    {
        return !isBoss &&
            canFlee &&
            (allowRepeatFlee || !HasRetreated) &&
            health != null &&
            health.Core.CurrentHealth <= health.Core.MaxHealth * fleeHealthThreshold;
    }

    public virtual bool CanStartChase()
    {
        if (PeacefulModeService.IsEnabled)
        {
            if (isBoss)
            {
                return IsProvoked;
            }

            return false;
        }

        if (isBoss)
        {
            return true;
        }

        return !isPeaceful;
    }

    public virtual bool ShouldAbortCombat()
    {
        if (!PeacefulModeService.IsEnabled)
        {
            return false;
        }

        if (isBoss)
        {
            return !IsProvoked;
        }

        return !ShouldRetreat();
    }

    public virtual bool ShouldEnterEnragedState()
    {
        return isBoss &&
            !HasEnteredEnragedState &&
            health != null &&
            health.Core.CurrentHealth <= health.Core.MaxHealth * 0.5f;
    }

    public virtual float GetFlatDistanceToTarget()
    {
        if (target == null)
        {
            return float.MaxValue;
        }

        Vector3 flatTarget = target.position;
        flatTarget.y = transform.position.y;
        return Vector3.Distance(transform.position, flatTarget);
    }

    public virtual float GetCombatDistance()
    {
        return Mathf.Max(attackRange, stoppingDistance);
    }

    public IState ChooseAttackState()
    {
        return _attackSelector.GetNextAttackState();
    }

    public void AlertEnemy()
    {
        IsProvoked = true;
    }

    public void CompleteRetreat()
    {
        HasRetreated = true;
    }

    public void MarkEnragedStateEntered()
    {
        HasEnteredEnragedState = true;
    }

    public void ResetBehaviorState()
    {
        IsProvoked = !isBoss;
        HasRetreated = false;
        HasEnteredEnragedState = false;
        TrackedTargetPosition = transform.position;
    }

    public bool CanDamageTarget(float distance)
    {
        if (target == null || targetDamageable == null || !targetDamageable.IsAlive)
        {
            ResolveTargetReferences();
        }

        return targetDamageable != null &&
            targetDamageable.IsAlive &&
            distance <= GetCombatDistance();
    }

    public virtual void PerformAttack()
    {
        anim?.PlayAttack();

        if (CanDamageTarget(GetFlatDistanceToTarget()))
        {
            targetDamageable.TakeDamage(attackDamage, 0f);
        }
    }

    public virtual void PerformPowerAttack()
    {
        anim?.PlayPowerAttack();

        if (CanDamageTarget(GetFlatDistanceToTarget()))
        {
            targetDamageable.TakeDamage(powerAttackDamage, 0f);
        }
    }

    public virtual void ApplySpawnStats(float newAttackDamage, float newPowerAttackDamage, float newAttackRange, float newStoppingDistance)
    {
        attackDamage = newAttackDamage;
        powerAttackDamage = newPowerAttackDamage;
        attackRange = newAttackRange;
        stoppingDistance = newStoppingDistance;

        if (agent != null)
        {
            agent.stoppingDistance = stoppingDistance;
        }
    }

    public virtual void ApplyRareState(bool isRare)
    {
        RareVisualController rareVisualController = GetComponent<RareVisualController>();
        rareVisualController?.SetRareState(isRare);
    }

    public virtual void Construct(IAudioService audioService)
    {
        if (health == null)
        {
            health = GetComponent<HealthComponent>();
        }

        health?.Construct(audioService);
    }

    public virtual EnemySaveData CaptureState()
    {
        _enemyId ??= GetComponent<EnemyId>();

        if (health == null)
        {
            health = GetComponent<HealthComponent>();
        }

        if (_enemyId == null || health == null || health.Core == null)
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

    public virtual void RestoreState(IReadOnlyList<EnemySaveData> enemyStates)
    {
        _enemyId ??= GetComponent<EnemyId>();

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

        if (_enemyId == null || health == null || agent == null || anim == null || health.Core == null)
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
        StateMachine.ChangeState(new IdleState(this));
    }

    public virtual void FaceTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void HandleDamaged()
    {
        if (isBoss)
        {
            IsProvoked = true;
        }
    }

    private void RememberTargetPosition()
    {
        if (target == null)
        {
            return;
        }

        TrackedTargetPosition = target.position;
    }

    private void ResolveTargetReferences()
    {
        if (target == null)
        {
            target = FindPlayerTarget();
        }

        if (target != null)
        {
            targetDamageable = target.GetComponent<IDamageable>() ??
                target.GetComponentInParent<IDamageable>();
        }
    }

    private static Transform FindPlayerTarget()
    {
        PlayerCombat playerCombat = FindFirstObjectByType<PlayerCombat>();
        if (playerCombat != null)
        {
            return playerCombat.transform;
        }

        PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null)
        {
            return playerMovement.transform;
        }

        GameObject taggedPlayer = GameObject.FindGameObjectWithTag("Player");
        if (taggedPlayer != null)
        {
            return taggedPlayer.transform;
        }

        HealthComponent playerHealth = FindFirstObjectByType<HealthComponent>();
        return playerHealth != null ? playerHealth.transform : null;
    }
}
