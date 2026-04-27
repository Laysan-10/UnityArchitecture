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

    [Header("References")]
    public NavMeshAgent agent;
    public EnemyAnimationBase anim;
    public HealthComponent health;
    public Transform target;
    public HealthBarUI healthBarUi;
    public IDamageable targetDamageable;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<EnemyAnimationBase>();
        health = GetComponent<HealthComponent>();
        healthBarUi = GetComponentInChildren<HealthBarUI>(true);
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

        StateMachine.ChangeState(new IdleState(this));
    }

    protected virtual void Update()
    {
        if (health != null && health.Core.IsDead)
        {
            if (!(StateMachine.CurrentState is DeadState))
            {
                StateMachine.ChangeState(new DeadState(this));
            }

            return;
        }

        StateMachine.Update();
    }

    public float GetAttackSpeed()
    {
        if (isBoss && health.Core.CurrentHealth < health.Core.MaxHealth * 0.5f)
        {
            return 0.5f;
        }

        return attackCooldown;
    }

    public virtual bool ShouldFlee()
    {
        return !isBoss &&
            health != null &&
            health.Core.CurrentHealth <= health.Core.MaxHealth * fleeHealthThreshold;
    }

    public virtual bool CanAggroByProximity()
    {
        return !isPeaceful;
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
}
