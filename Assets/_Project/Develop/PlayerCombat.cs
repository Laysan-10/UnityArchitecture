using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Настройки Физической Атаки")]
    [SerializeField] private float physDamage = 25f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private Transform attackPoint; // Точка перед игроком (откуда бьем)
    [SerializeField] private LayerMask enemyLayer;  // Слой врагов (чтобы не бить всё подряд)

    [Header("Настройки Магической Атаки")]
    [SerializeField] private float magicDamage = 40f;
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform firePoint;   // Откуда вылетает фаербол
    [SerializeField] private float magicCooldown = 5f;

    private IInputService _inputService;
    private float _magicCooldownTimer;

    // Свойство проверки готовности магии
    public bool IsMagicReady => _magicCooldownTimer <= 0;

    // События для Аниматора
    public event Action OnAttackPhysFired;
    public event Action OnAttackMagFired;

    // События для UI (Инверсия зависимостей!)
    public event Action OnMagicCooldownStarted;
    public event Action<float> OnMagicCooldownTick; // Передает оставшееся время
    public event Action OnMagicCooldownFinished;

    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
        _inputService.OnPhysicalAttack += HandlePhysicalAttack;
        _inputService.OnMagicAttack += HandleMagicAttack;
    }

    private void Update()
    {
        // Логика таймера кулдауна
        if (_magicCooldownTimer > 0)
        {
            _magicCooldownTimer -= Time.deltaTime;
            OnMagicCooldownTick?.Invoke(_magicCooldownTimer);

            if (_magicCooldownTimer <= 0)
            {
                OnMagicCooldownFinished?.Invoke();
            }
        }
    }

    private void HandlePhysicalAttack()
    {
        OnAttackPhysFired?.Invoke(); // Запускаем анимацию

        // Создаем сферу перед игроком и ищем все объекты на слое Enemy
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            // Проверяем, есть ли на объекте интерфейс IDamageable
            if (enemy.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(physDamage, 0); // Наносим физ. урон
                Debug.Log($"Нанесен физический урон объекту {enemy.name}!");
            }
        }
    }

    private void HandleMagicAttack()
    {
        if (!IsMagicReady)
        {
            Debug.Log("Магия на кулдауне!");
            return;
        }

        OnAttackMagFired?.Invoke(); // Запускаем анимацию

        // Спавним фаербол
        if (magicPrefab != null && firePoint != null)
        {
            GameObject fireball = Instantiate(magicPrefab, firePoint.position, firePoint.rotation);
            
            // Настраиваем урон фаербола при создании
            if (fireball.TryGetComponent<MagicProjectile>(out var projectile))
            {
                projectile.Setup(magicDamage);
            }
        }

        // Запускаем кулдаун
        _magicCooldownTimer = magicCooldown;
        OnMagicCooldownStarted?.Invoke();
    }

    // Для удобства настройки в редакторе (рисует радиус атаки мечом)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnDestroy()
    {
        if (_inputService != null)
        {
            _inputService.OnPhysicalAttack -= HandlePhysicalAttack;
            _inputService.OnMagicAttack -= HandleMagicAttack;
        }
    }
}