using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyAnimationController))]
public class newEnemyAI : MonoBehaviour
{
    [Header("Настройки ИИ")]
    public float lookRadius = 10f;
    public float attackCooldown = 1.5f;
    [SerializeField] private float damageAmount = 15f;
    
    [Header("Ссылки")]
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
        // Убрали прятание UI отсюда. Теперь тут только логика ИИ!
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

                if (Time.time - _lastAttackTime >= attackCooldown)
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

        if (_targetDamageable != null)
        {
            _targetDamageable.TakeDamage(damageAmount, 0);
        }
    }

    private void LookTarget()
    {
        Vector3 direction = (targetTransform.position - transform.position).normalized;
        direction.y = 0; 
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}