using System;
using UnityEngine;
using UnityEngine.AI;
public class newEnemyAI : MonoBehaviour
{
 [SerializeField] private Transform targetTransform;
    private IDamageable target;  // Зависимость от абстракции (DIP)
    
    private NavMeshAgent agent;
    public Animator animator;
    public float LookRadius;
    public float attackCooldown = 1f;
    [SerializeField] private float damageAmount = 10f;  // Урон врага
    private float lastAttackTime;
    private EnemyAnimationController _animController;
     private bool _isRunning;
    public bool IsRunning => _isRunning;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
        
        // Получаем интерфейс из Transform (DIP: не жесткая связь с PlayerHealth)
        if (targetTransform != null)
        {
            target = targetTransform.GetComponent<IDamageable>();
            _animController = GetComponent<EnemyAnimationController>();
            if (_animController != null)
        {
            _animController.Construct(this, target);
        }
        else
        {
            Debug.LogError($"EnemyAnimationController не найден на объекте {gameObject.name}!");
        }
        }
    }

       private void Update()
{
    if (targetTransform == null) return;

    float distance = Vector3.Distance(targetTransform.position, transform.position);

    if (distance < LookRadius)
    {
        // ПРОВЕРКА: Если мы уже подошли на расстояние удара
        if (distance <= agent.stoppingDistance)
        {
            // 1. ЛОГИКА: Выключаем бег для аниматора
            _isRunning = false; 

            // 2. ФИЗИКА: Полностью останавливаем агента
            agent.isStopped = true;       // Отключаем расчет пути
            agent.velocity = Vector3.zero; // Обнуляем инерцию (чтобы не скользил)

            LookTarget(); // Поворачиваемся к игроку

            // 3. АТАКА: Проверка кулдауна
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                _animController?.PlayAttack();
            }
        }
        else 
        {
            // ПРОВЕРКА: Если мы еще далеко — преследуем
            _isRunning = true;
            agent.isStopped = false; // Разрешаем движение
            agent.SetDestination(targetTransform.position);
        }
    }
    else
    {
        // Игрок вышел из радиуса видимости
        _isRunning = false;
        agent.isStopped = true;
    }
}

    void LookTarget()
    {
        Vector3 direction = (targetTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);  // Улучшено
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}