using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    private newEnemyAI _ai;  // Аналог PlayerMovement
    private IDamageable _healthComponent;  // Аналог HealthComponent

    // Хеши параметров анимаций (оптимизация, как в PlayerAnimationController)
    private static readonly int IsRunHash = Animator.StringToHash("IsRun");
    private static readonly int IsAttackHash = Animator.StringToHash("IsAttack");
    private static readonly int HitHash = Animator.StringToHash("IsHit");
    private static readonly int DeadHash = Animator.StringToHash("death");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Внедрение зависимостей (DIP - аналогично игроку)
    public void Construct(newEnemyAI ai, IDamageable health)
    {
        _ai = ai;
        _healthComponent = health;

        // Подписка на события урона (если healthComponent имеет события OnDamaged/OnDeath)
        if (_healthComponent != null)
        {
            // Предполагаем, что IDamageable имеет события (расширьте интерфейс при необходимости)
            // _healthComponent.OnDamaged += PlayHit;
            // _healthComponent.OnDeath += PlayDead;
        }
    }

    private void Update()
    {
        if (_ai == null) return;

        // Плавный переход IsRun (0 или 1) вместо резкой смены
        float currentRun = _animator.GetBool(IsRunHash) ? 1f : 0f;
        float targetRun = _ai.IsRunning ? 1f : 0f;  // Нужно добавить в newEnemyAI
        
        float smoothRun = Mathf.MoveTowards(currentRun, targetRun, 5f * Time.deltaTime);
        _animator.SetBool(IsRunHash, smoothRun > 0.5f);
    }

    // Вызывается из newEnemyAI при атаке (или через событие)
    public void PlayAttack()
    {
        _animator.SetTrigger(IsAttackHash);
    }

    // Вызывается системой Health (DIP)
    public void PlayHit()
    {
        _animator.SetTrigger(HitHash);
    }

    public void PlayDead()
    {
        _animator.SetTrigger(DeadHash);
    }

    private void OnDestroy()
    {
        // Отписка от событий (чистый код)
        if (_healthComponent != null)
        {
            // _healthComponent.OnDamaged -= PlayHit;
            // _healthComponent.OnDeath -= PlayDead;
        }
    }
}
