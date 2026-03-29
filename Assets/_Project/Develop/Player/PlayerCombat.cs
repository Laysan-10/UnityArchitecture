using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Physical Attack Settings")]
    [SerializeField] private float physDamage = 25f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private Transform attackPoint; 
    [SerializeField] private LayerMask enemyLayer; 

    [Header("Magic Attack Settings")]
    [SerializeField] private float magicDamage = 40f;
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform firePoint; 
    [SerializeField] private float magicCooldown = 5f;

    private IInputService _inputService;
    private float _magicCooldownTimer;

    public bool IsMagicReady => _magicCooldownTimer <= 0;

    public event Action OnAttackPhysFired;
    public event Action OnAttackMagFired;

    public event Action OnMagicCooldownStarted;
    public event Action<float> OnMagicCooldownTick; 
    public event Action OnMagicCooldownFinished;

    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
        _inputService.OnPhysicalAttack += HandlePhysicalAttack;
        _inputService.OnMagicAttack += HandleMagicAttack;
    }

    private void Update()
    {
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
        OnAttackPhysFired?.Invoke();
        ProjectBootstrapper.Instance.AudioService.PlaySound("Player_Swing");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(physDamage, 0);
                Debug.Log($"Нанесен физический урон объекту {enemy.name}!");
                // Звук попадания по врагу
                ProjectBootstrapper.Instance.AudioService.PlaySound("Enemy_Hit");
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

        OnAttackMagFired?.Invoke();
        ProjectBootstrapper.Instance.AudioService.PlaySound("Magic_Cast");

        if (magicPrefab != null && firePoint != null)
        {
            GameObject fireball = Instantiate(magicPrefab, firePoint.position, firePoint.rotation);
            
            if (fireball.TryGetComponent<MagicProjectile>(out var projectile))
            {
                projectile.Setup(magicDamage, true); 
            }
        }

        _magicCooldownTimer = magicCooldown;
        OnMagicCooldownStarted?.Invoke();
    }

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