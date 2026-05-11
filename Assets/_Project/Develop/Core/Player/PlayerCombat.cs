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
    private IAudioService _audioService;
    private float _magicCooldownTimer;
    private HealthComponent _playerHealth;

    public bool IsMagicReady => _magicCooldownTimer <= 0;

    public event Action OnAttackPhysFired;
    public event Action OnAttackMagFired;

    public event Action OnMagicCooldownStarted;
    public event Action<float> OnMagicCooldownTick;
    public event Action OnMagicCooldownFinished;

    public void Construct(IInputService inputService, HealthComponent health, IAudioService audioService)
    {
        _inputService = inputService;
        _playerHealth = health;
        _audioService = audioService;
        _inputService.OnPhysicalAttack += HandlePhysicalAttack;
        _inputService.OnMagicAttack += HandleMagicAttack;
    }

    private void Update()
    {
        if (Time.timeScale <= 0) return;

        if (_playerHealth != null && !_playerHealth.IsAlive) return;

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
        if (Time.timeScale <= 0 || (_playerHealth != null && !_playerHealth.IsAlive)) return;

        OnAttackPhysFired?.Invoke();
        _audioService?.PlaySound("Player_Swing");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(physDamage, 0);
                Debug.Log($"Dealt physical damage to {enemy.name}.");
                _audioService?.PlaySound("Enemy_Hit");
            }
        }
    }

    private void HandleMagicAttack()
    {
        if (Time.timeScale <= 0 || (_playerHealth != null && !_playerHealth.IsAlive)) return;

        if (!IsMagicReady)
        {
            Debug.Log("Magic is on cooldown.");
            return;
        }

        OnAttackMagFired?.Invoke();
        _audioService?.PlaySound("Magic_Cast");

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
