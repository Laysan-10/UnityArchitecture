using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private float _targetMoveSpeed;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int PhysicalAttackHash = Animator.StringToHash("PhysicalAttack");
    private static readonly int MagicAttackHash = Animator.StringToHash("MagicAttack");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMoveSpeed(float speed)
    {
        _targetMoveSpeed = speed;
    }

    public void PlayPhysicalAttack()
    {
        _animator.SetTrigger(PhysicalAttackHash);
    }

    public void PlayMagicAttack()
    {
        _animator.SetTrigger(MagicAttackHash);
    }

    public void PlayHit()
    {
        _animator.SetTrigger(HitHash);
    }

    public void PlayDead()
    {
        _animator.SetTrigger(DeadHash);
    }

    private void Update()
    {
        float currentAnimSpeed = _animator.GetFloat(SpeedHash);
        float smoothedSpeed = Mathf.MoveTowards(currentAnimSpeed, _targetMoveSpeed, 3f * Time.deltaTime);
        _animator.SetFloat(SpeedHash, smoothedSpeed);
    }
}
