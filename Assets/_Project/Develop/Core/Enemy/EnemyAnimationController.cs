using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : EnemyAnimationBase
{
    private Animator _animator;
    private EnemyBase _ai;
    private HealthCore _healthCore;

    private static readonly int IsRunHash = Animator.StringToHash("IsRun");
    private static readonly int IsAttackHash = Animator.StringToHash("IsAttack");
    private static readonly int HitHash = Animator.StringToHash("IsHit");
    private static readonly int DeadHash = Animator.StringToHash("IsDead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Construct(EnemyBase ai, HealthCore healthCore)
    {
        _ai = ai;
        _healthCore = healthCore;

        if (_healthCore != null)
        {
            _healthCore.OnDamaged += PlayHit;
            _healthCore.OnDeath += PlayDead;
        }
    }

    public override void SetRunning(bool isRunning)
    {
        _animator.SetBool(IsRunHash, isRunning);
    }

    public override void PlayAttack()
    {
        _animator.SetTrigger(IsAttackHash);
    }

    public override void PlayHit()
    {
        _animator.SetTrigger(HitHash);
    }

    public override void PlayDead()
    {
        _animator.SetTrigger(DeadHash);

        if (_ai != null)
        {
            _ai.enabled = false;
        }

        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
        {
            agent.enabled = false;
        }
    }

    public override void ResetVisuals()
    {
        _animator.Rebind();
        _animator.Update(0f);

        if (_ai != null)
        {
            _ai.enabled = true;
        }

        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
        {
            agent.enabled = true;
        }
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
