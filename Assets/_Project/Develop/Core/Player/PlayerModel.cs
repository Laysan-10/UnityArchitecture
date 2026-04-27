using System;

public class PlayerModel
{
    private float _currentHealth;
    private float _maxHealth;
    private bool _isDead;
    private float _moveAnimationSpeed;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public bool IsDead => _isDead;
    public float MoveAnimationSpeed => _moveAnimationSpeed;

    public event Action<float> OnMoveAnimationSpeedChanged;
    public event Action<float, float> OnHealthChanged;
    public event Action OnDamaged;
    public event Action OnDeath;

    public void SetMoveAnimationSpeed(float speed)
    {
        if (Math.Abs(_moveAnimationSpeed - speed) < 0.001f)
        {
            return;
        }

        _moveAnimationSpeed = speed;
        OnMoveAnimationSpeedChanged?.Invoke(_moveAnimationSpeed);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
        _isDead = currentHealth <= 0f;

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void NotifyDamaged()
    {
        OnDamaged?.Invoke();
    }

    public void NotifyDeath()
    {
        if (_isDead)
        {
            OnDeath?.Invoke();
            return;
        }

        _isDead = true;
        OnDeath?.Invoke();
    }
}
