using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    private newEnemyAI _ai;
    private HealthCore _healthCore; 

    private static readonly int IsRunHash = Animator.StringToHash("IsRun");
    private static readonly int IsAttackHash = Animator.StringToHash("IsAttack");
    private static readonly int HitHash = Animator.StringToHash("IsHit");
    private static readonly int DeadHash = Animator.StringToHash("IsDead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Construct(newEnemyAI ai, HealthCore healthCore)
    {
        _ai = ai;
        _healthCore = healthCore;

        if (_healthCore != null)
        {
            _healthCore.OnDamaged += PlayHit;
            _healthCore.OnDeath += PlayDead;
        }
    }

    private void Update()
    {
        if (_ai == null || (_healthCore != null && _healthCore.IsDead)) return;

        _animator.SetBool(IsRunHash, _ai.IsRunning);
    }

    public void PlayAttack() => _animator.SetTrigger(IsAttackHash);
    private void PlayHit() => _animator.SetTrigger(HitHash);
    public void SetRunning(bool value) => _animator.SetBool(IsRunHash, value);
    
    private void PlayDead() 
    {
        _animator.SetTrigger(DeadHash);
        if (_ai != null) _ai.enabled = false;
        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent)) agent.enabled = false;
    }

    public void ResetVisuals()
    {
        _animator.Rebind();
        _animator.Update(0f);
        
        if (_ai != null) _ai.enabled = true;
        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent)) agent.enabled = true;
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