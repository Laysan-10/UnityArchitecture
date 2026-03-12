using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _movement;
    private PlayerCombat _combat;
    private HealthComponent _healthComponent;


    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int PhysicalAttackHash = Animator.StringToHash("PhysicalAttack");
    private static readonly int MagicAttackHash = Animator.StringToHash("MagicAttack");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Внедрение зависимостей логики в аниматор
    public void Construct(PlayerMovement movement, PlayerCombat combat, HealthComponent health)
    {
        _movement = movement;
        _combat = combat;
        _healthComponent = health;

        // Подписываемся на события боя
        _combat.OnAttackPhysFired += PlayPhysAttack;
        _combat.OnAttackMagFired += PlayMagAttack;

        // Подписки на получение урона (Инверсия зависимостей - Аниматор сам слушает здоровье!)
        if (_healthComponent != null)
        {
            _healthComponent.Core.OnDamaged += PlayHit;
            _healthComponent.Core.OnDeath += PlayDead;
        }

    }

    private void Update()
    {
        if (_movement == null) return;

        // Плавный переход анимации (Dampening), чтобы персонаж не дергался при смене стейтов
        // Mathf.MoveTowards плавно меняет текущее значение Speed в Аниматоре к целевому (0, 0.4 или 0.8)
        float currentAnimSpeed = _animator.GetFloat(SpeedHash);
        float targetAnimSpeed = _movement.CurrentAnimationSpeed;
        
        _animator.SetFloat(SpeedHash, Mathf.MoveTowards(currentAnimSpeed, targetAnimSpeed, 3f * Time.deltaTime));
    }

    private void PlayPhysAttack() => _animator.SetTrigger(PhysicalAttackHash);
    private void PlayMagAttack() => _animator.SetTrigger(MagicAttackHash);

    // Эти методы вызовет система Health в будущем (инверсия зависимостей!)
    public void PlayHit() => _animator.SetTrigger(HitHash);
    public void PlayDead() => _animator.SetTrigger(DeadHash);

    private void OnDestroy()
    {
        if (_combat != null)
        {
            _combat.OnAttackPhysFired -= PlayPhysAttack;
            _combat.OnAttackMagFired -= PlayMagAttack;
        }

        if (_healthComponent != null)
        {
            _healthComponent.Core.OnDamaged -= PlayHit;
            _healthComponent.Core.OnDeath -= PlayDead;
        }
    }
}