public class PlayerController
{
    private readonly PlayerModel _model;
    private readonly PlayerAnimationController _view;
    private readonly PlayerMovement _movement;
    private readonly PlayerCombat _combat;
    private readonly HealthComponent _health;

    public PlayerController(
        PlayerModel model,
        PlayerAnimationController view,
        PlayerMovement movement,
        PlayerCombat combat,
        HealthComponent health)
    {
        _model = model;
        _view = view;
        _movement = movement;
        _combat = combat;
        _health = health;

        BindModelToView();
        BindGameplayToModel();
        InitializeModelState();
    }

    public void Tick()
    {
        _model.SetMoveAnimationSpeed(_movement.CurrentAnimationSpeed);
    }

    public void Dispose()
    {
        _model.OnMoveAnimationSpeedChanged -= _view.SetMoveSpeed;
        _model.OnDamaged -= _view.PlayHit;
        _model.OnDeath -= _view.PlayDead;

        _combat.OnAttackPhysFired -= _view.PlayPhysicalAttack;
        _combat.OnAttackMagFired -= _view.PlayMagicAttack;

        if (_health.Core != null)
        {
            _health.Core.OnHealthChanged -= HandleHealthChanged;
            _health.Core.OnDamaged -= HandleDamaged;
            _health.Core.OnDeath -= HandleDeath;
        }
    }

    private void BindModelToView()
    {
        _model.OnMoveAnimationSpeedChanged += _view.SetMoveSpeed;
        _model.OnDamaged += _view.PlayHit;
        _model.OnDeath += _view.PlayDead;
    }

    private void BindGameplayToModel()
    {
        _combat.OnAttackPhysFired += _view.PlayPhysicalAttack;
        _combat.OnAttackMagFired += _view.PlayMagicAttack;

        if (_health.Core != null)
        {
            _health.Core.OnHealthChanged += HandleHealthChanged;
            _health.Core.OnDamaged += HandleDamaged;
            _health.Core.OnDeath += HandleDeath;
        }
    }

    private void InitializeModelState()
    {
        if (_health.Core != null)
        {
            _model.SetHealth(_health.Core.CurrentHealth, _health.Core.MaxHealth);
        }

        _model.SetMoveAnimationSpeed(_movement.CurrentAnimationSpeed);
    }

    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        _model.SetHealth(currentHealth, maxHealth);
    }

    private void HandleDamaged()
    {
        _model.NotifyDamaged();
    }

    private void HandleDeath()
    {
        _model.NotifyDeath();
    }
}
