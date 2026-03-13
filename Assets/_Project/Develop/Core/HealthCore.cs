using System;
using UnityEngine;

public class HealthCore
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    // События, на которые подпишутся UI, Аниматор и ИИ
    public event Action<float, float> OnHealthChanged; // Передает (Текущее HP, Макс HP)
    public event Action OnDamaged; // Вызывается для стана/анимации получения урона
    public event Action OnDeath; // Вызывается при смерти

    public HealthCore(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void ApplyDamage(float physical, float magic)
    {
        if (IsDead) return;

        // В будущем тут можно добавить броню (например: physical - armor)
        float totalDamage = physical + magic;
        
        CurrentHealth = Mathf.Clamp(CurrentHealth - totalDamage, 0, MaxHealth);
        
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth > 0)
        {
            OnDamaged?.Invoke(); // Герой жив, но получил по лицу
        }
        else
        {
            OnDeath?.Invoke(); // Герой умер
        }
    }
}