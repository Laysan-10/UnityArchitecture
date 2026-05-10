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

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<EnemyAnimationBase>();
        health = GetComponent<HealthComponent>();
        healthBarUi = GetComponentInChildren<HealthBarUI>(true);
        _attackSelector = new EnemyAttackSelector(this);
    }

    protected virtual void Start()
    {
        StateMachine = new EnemyStateMachine();

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        if (target != null)
        {
            targetDamageable = target.GetComponent<IDamageable>();
        }

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
}
