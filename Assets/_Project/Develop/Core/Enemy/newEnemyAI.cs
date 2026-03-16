using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyAnimationController))]
public class newEnemyAI : MonoBehaviour
{
    public enum EnemyType { Melee, Ranged }

    [Header("Basic settings")]
    public EnemyType type;
    public float lookRadius = 15f;      
    public float attackRange = 2.5f;
    public float attackCooldown = 1.5f;
    [SerializeField] private float damageAmount = 15f;

    [Header("Для дальнего боя (Ranged)")]
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform firePoint;

    [Header("References")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private HealthBarUI enemyHealthBarUI;

    private NavMeshAgent _agent;
    private EnemyAnimationController _animController;
    private HealthComponent _myHealth;
    private IDamageable _targetDamageable;
    private float _lastAttackTime;
    
    public bool IsRunning { get; private set; }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animController = GetComponent<EnemyAnimationController>();
        _myHealth = GetComponent<HealthComponent>(); 

        if (_animController != null && _myHealth != null)
        {
            _animController.Construct(this, _myHealth.Core);
        }

        if (enemyHealthBarUI != null && _myHealth != null)
        {
            enemyHealthBarUI.Construct(_myHealth.Core);
        }

        if (targetTransform != null)
        {
            _targetDamageable = targetTransform.GetComponent<IDamageable>();
        }
    }

    private void Update()
    {
        if (targetTransform == null || (_myHealth != null && _myHealth.Core.IsDead)) 
        {
            IsRunning = false;
            _agent.isStopped = true;
            return;
        }

        float distance = Vector3.Distance(targetTransform.position, transform.position);

        if (distance <= lookRadius)
        {
            if (distance <= _agent.stoppingDistance)
            {
                IsRunning = false;
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;

                LookTarget();

                if (distance <= attackRange && Time.time - _lastAttackTime >= attackCooldown)
                {
                    Attack();
                }
            }
            else 
            {
                IsRunning = true;
                _agent.isStopped = false;
                _agent.SetDestination(targetTransform.position);
            }
        }
        else
        {
            IsRunning = false;
            _agent.isStopped = true;
        }
    }

    private void Attack()
    {
        _lastAttackTime = Time.time;
        _animController?.PlayAttack();

        if (type == EnemyType.Melee)
        {
            if (_targetDamageable != null)
            {
                _targetDamageable.TakeDamage(damageAmount, 0);
            }
        }
        else if (type == EnemyType.Ranged)
        {
            if (magicPrefab != null && firePoint != null)
            {
                Vector3 aimDirection = (targetTransform.position + Vector3.up * 1f) - firePoint.position;
                GameObject fireball = Instantiate(magicPrefab, firePoint.position, Quaternion.LookRotation(aimDirection));
                
                if (fireball.TryGetComponent<MagicProjectile>(out var projectile))
                {
                    projectile.Setup(damageAmount, false);
                }
            }
        }
    }

    private void LookTarget()
    {
        Vector3 direction = (targetTransform.position - transform.position).normalized;
        direction.y = 0; 
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}