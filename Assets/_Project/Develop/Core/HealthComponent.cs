using UnityEngine;

// Реализуем интерфейс! Теперь любой, кто бьет этот объект, не знает про HealthComponent.
public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    
    // Ссылка на чистую логику
    public HealthCore Core { get; private set; }

    private void Awake()
    {
        // Создаем ядро здоровья
        Core = new HealthCore(maxHealth);
    }

    // Этот метод вызовет меч или фаербол
    public void TakeDamage(float physicalDamage, float magicDamage)
    {
        Core.ApplyDamage(physicalDamage, magicDamage);
    }
}