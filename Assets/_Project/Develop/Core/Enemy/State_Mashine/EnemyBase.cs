using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BossAnimationController))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(Animator))]
public class EnemyBase : MonoBehaviour
{
    public EnemyStateMachine StateMachine;

    [Header("Settings")]
    public float lookRadius = 15f;
    public float attackRange = 2.5f;
    public float attackCooldown = 1.5f;
    public float stoppingDistance = 6f;
    public float attackDamage = 15f;
    public float powerAttackDamage = 30f;
    public bool isPeaceful = false;
    public bool isBoss = false;

    [Header("References")]
    public NavMeshAgent agent;
    public BossAnimationController anim;
    public HealthComponent health;
    public Transform target;
    public HealthBarUI healthBarUi;
    public IDamageable targetDamageable;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<BossAnimationController>();
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

    private void Update() => StateMachine.Update();

    public float GetAttackSpeed()
    {
        if (isBoss && health.Core.CurrentHealth < health.Core.MaxHealth * 0.5f)
        {
            return 0.5f;
        }

        return attackCooldown;
    }

    public bool CanDamageTarget(float distance)
    {
        return targetDamageable != null && targetDamageable.IsAlive && distance <= Mathf.Max(attackRange, stoppingDistance);
    }
}
