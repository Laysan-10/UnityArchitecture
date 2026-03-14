using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    private newEnemyAI _ai;
    private HealthCore _healthCore; // Слушаем чистое ядро здоровья САМОГО МОБА

    private static readonly int IsRunHash = Animator.StringToHash("IsRun");
    private static readonly int IsAttackHash = Animator.StringToHash("IsAttack");
    private static readonly int HitHash = Animator.StringToHash("IsHit");
    private static readonly int DeadHash = Animator.StringToHash("IsDead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Внедряем ИИ и ЗДОРОВЬЕ ВРАГА (а не игрока)
    public void Construct(newEnemyAI ai, HealthCore healthCore)
    {
        _ai = ai;
        _healthCore = healthCore;

        // Подписка на урон по врагу
        if (_healthCore != null)
        {
            _healthCore.OnDamaged += PlayHit;
            _healthCore.OnDeath += PlayDead;
        }
    }

    private void Update()
    {
        if (_ai == null || (_healthCore != null && _healthCore.IsDead)) return;

        // БЕЗ сглаживания. Если ИИ говорит бежать - бежим мгновенно.
        _animator.SetBool(IsRunHash, _ai.IsRunning);
    }

    public void PlayAttack() => _animator.SetTrigger(IsAttackHash);
    private void PlayHit() => _animator.SetTrigger(HitHash);
    
    private void PlayDead() 
    {
        _animator.SetTrigger(DeadHash);
        // Отключаем ИИ и Навмеш, чтобы труп не полз за игроком
        if (_ai != null) _ai.enabled = false;
        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent)) agent.enabled = false;
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