using System.Collections.Generic;
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

    [Header("Ranged Attack")]
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform firePoint;

    [Header("References")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private HealthBarUI enemyHealthBarUI;

    private NavMeshAgent _agent;
    private EnemyAnimationController _animController;
    private HealthComponent _myHealth;
    private IAudioService _audioService;
    private IDamageable _targetDamageable;
    private float _lastAttackTime;
    private EnemyId _enemyId;

    public bool IsRunning { get; private set; }

    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;

        if (_myHealth == null)
        {
            _myHealth = GetComponent<HealthComponent>();
        }

        _myHealth?.Construct(audioService);
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animController = GetComponent<EnemyAnimationController>();
        _myHealth = GetComponent<HealthComponent>();
        _enemyId = GetComponent<EnemyId>();

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
        bool isTargetAlive = _targetDamageable != null && _targetDamageable.IsAlive;

        if (targetTransform == null || (_myHealth != null && _myHealth.Core.IsDead) || !isTargetAlive)
        {
            IsRunning = false;
            if (_agent.isActiveAndEnabled) _agent.isStopped = true;
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
            _audioService?.PlaySound("Enemy_Attack_Melee");
            if (_targetDamageable != null)
            {
                _targetDamageable.TakeDamage(damageAmount, 0);
                _audioService?.PlaySound("Player_Hit");
            }
        }
        else if (type == EnemyType.Ranged)
        {
            _audioService?.PlaySound("Enemy_Attack_Ranged");
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

    public EnemySaveData CaptureState()
    {
        if (_enemyId == null)
        {
            _enemyId = GetComponent<EnemyId>();
        }

        if (_myHealth == null)
        {
            _myHealth = GetComponent<HealthComponent>();
        }

        if (_enemyId == null || _myHealth == null)
        {
            return null;
        }

        return new EnemySaveData
        {
            Id = _enemyId.Id,
            Position = transform.position,
            CurrentHp = _myHealth.Core.CurrentHealth,
            IsDead = _myHealth.Core.IsDead
        };
    }

    public void RestoreState(IReadOnlyList<EnemySaveData> enemyStates)
    {
        if (_enemyId == null)
        {
            _enemyId = GetComponent<EnemyId>();
        }

        if (_myHealth == null)
        {
            _myHealth = GetComponent<HealthComponent>();
        }

        if (_agent == null)
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        if (_animController == null)
        {
            _animController = GetComponent<EnemyAnimationController>();
        }

        if (_enemyId == null || _myHealth == null || _agent == null || _animController == null)
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
            _myHealth.Core.RestoreHealth(0);
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (enemyHealthBarUI != null)
        {
            enemyHealthBarUI.gameObject.SetActive(true);
        }

        _myHealth.Core.RestoreHealth(myData.CurrentHp);

        _agent.enabled = false;
        transform.position = myData.Position;
        _agent.enabled = true;
        _agent.isStopped = false;

        _animController.ResetVisuals();
        _lastAttackTime = Time.time;
    }
}
