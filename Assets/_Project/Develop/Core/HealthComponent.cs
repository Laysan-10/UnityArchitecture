using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
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
}