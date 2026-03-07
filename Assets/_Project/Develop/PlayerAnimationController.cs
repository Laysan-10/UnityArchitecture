using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _movement;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int SprintHash = Animator.StringToHash("Sprint");
    private static readonly int PhysicalAttackHash = Animator.StringToHash("PhysicalAttack");
    private static readonly int MagicAttackHash = Animator.StringToHash("MagicAttack");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateMovementAnimation();
    }

    private void UpdateMovementAnimation()
    {
        if (_movement == null) return;

        Vector3 velocity = _movement.Velocity;
        velocity.y = 0f;

        float speed = velocity.magnitude;

        float maxSpeed = 10f; // sprintSpeed
        float normalizedSpeed = speed / maxSpeed;

        _animator.SetFloat(SpeedHash, normalizedSpeed);
    }

    public void PlayPhysicalAttack()
    {
        _animator.SetTrigger(PhysicalAttackHash);
    }

    public void PlayMagicAttack()
    {
        _animator.SetTrigger(MagicAttackHash);
    }
}