using System;
using UnityEngine;

public class HealthCore
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public event Action<float, float> OnHealthChanged; 
    public event Action OnDamaged; 
    public event Action OnDeath; 

    public HealthCore(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void ApplyDamage(float physical, float magic)
    {
        if (IsDead) return;

        float totalDamage = physical + magic;
        
        CurrentHealth = Mathf.Clamp(CurrentHealth - totalDamage, 0, MaxHealth);
        
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth > 0)
        {
            OnDamaged?.Invoke(); 
        }
        else
        {
            OnDeath?.Invoke(); 
        }
    }

    public void RestoreHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(amount, 0, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

}