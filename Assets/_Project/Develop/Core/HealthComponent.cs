using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable, ISaveable
{
    [SerializeField] private float maxHealth = 100f;
    
    public HealthCore Core { get; private set; }

    private void Awake()
    {
        Core = new HealthCore(maxHealth);
    }

    public void TakeDamage(float physicalDamage, float magicDamage)
    {
        Core.ApplyDamage(physicalDamage, magicDamage);
    }

        public void PopulateSaveData(SaveData saveData) 
    {
        if (gameObject.CompareTag("Player")) // Сохраняем ХП только если это игрок
            saveData.PlayerHealth = Core.CurrentHealth;
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        if (gameObject.CompareTag("Player"))
            Core.ApplyDamage(-(saveData.PlayerHealth - Core.CurrentHealth), 0); // Хак, чтобы восстановить ХП через метод ApplyDamage
    }

}