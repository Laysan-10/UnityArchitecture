using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;

    public HealthCore Core { get; private set; }

    public bool IsAlive => Core != null && !Core.IsDead;
    public float CurrentHealth => Core != null ? Core.CurrentHealth : 0f;

    private void Awake()
    {
        Core = new HealthCore(maxHealth);
    }

    public void Construct(IAudioService audioService)
    {
        Core.SetAudioService(audioService);
    }

    public void TakeDamage(float physicalDamage, float magicDamage)
    {
        Core.ApplyDamage(physicalDamage, magicDamage);
    }

    public void SetMaxHealth(float health, bool restoreToFull = true)
    {
        Core.SetMaxHealth(health, restoreToFull);
    }

    public void RestoreHealth(float health)
    {
        Core.RestoreHealth(health);
    }
}
