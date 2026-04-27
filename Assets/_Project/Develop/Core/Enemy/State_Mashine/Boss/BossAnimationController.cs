using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Animator))]
public class BossAnimationController : MonoBehaviour
{
 
    private Animator _animator;
    private EnemyBase _ai; // Заменили на EnemyBase (новый контекст)
    private HealthCore _healthCore; 

    // Хэши параметров (оптимизация)
    private static readonly int IsRunHash = Animator.StringToHash("IsRun");
    private static readonly int IsAttackHash = Animator.StringToHash("IsAttack");
    private static readonly int IsPowerAttackHash = Animator.StringToHash("IsPowerAttack"); // Добавили сильную атаку
    private static readonly int HitHash = Animator.StringToHash("IsHit");
    private static readonly int DeadHash = Animator.StringToHash("IsDead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Construct(EnemyBase ai, HealthCore healthCore)
    {
        _ai = ai;
        _healthCore = healthCore;

        if (_healthCore != null)
        {
            // Подписываемся на события здоровья
            _healthCore.OnDamaged += PlayHit;
            _healthCore.OnDeath += PlayDead;
        }
    }

    // Этот метод теперь вызывает машина состояний
    public void SetRunning(bool isRunning)
    {
        _animator.SetBool(IsRunHash, isRunning);
    }

    // МЕТОДЫ ДЛЯ СОСТОЯНИЙ (должны быть PUBLIC)

    public void PlayAttack() => _animator.SetTrigger(IsAttackHash);
    
    public void PlayPowerAttack() => _animator.SetTrigger(IsPowerAttackHash); // Для босса

    public void PlayHit() => _animator.SetTrigger(HitHash); // Сделали PUBLIC
    
    public void PlayDead() // Сделали PUBLIC
    {
        _animator.SetTrigger(DeadHash);
        
        // Отключаем ИИ и навигацию при смерти
        if (_ai != null) _ai.enabled = false;
        if (TryGetComponent<NavMeshAgent>(out var agent)) agent.enabled = false;
    }

    public void ResetVisuals()
    {
        _animator.Rebind();
        _animator.Update(0f);
        
        if (_ai != null) _ai.enabled = true;
        if (TryGetComponent<NavMeshAgent>(out var agent)) agent.enabled = true;
    }

    private void OnDestroy()
    {
        if (_healthCore != null)
        {
            _healthCore.OnDamaged -= PlayHit;
            _healthCore.OnDeath -= PlayDead;
        }
    }
}

