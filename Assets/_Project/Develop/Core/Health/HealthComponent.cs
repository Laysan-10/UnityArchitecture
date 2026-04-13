using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable, ISaveable
{
    [SerializeField] private float maxHealth = 100f;

    public HealthCore Core { get; private set; }

    public bool IsAlive => Core != null && !Core.IsDead;

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

    public void PopulateSaveData(SaveData saveData)
    {
        if (gameObject.CompareTag("Player"))
            saveData.PlayerHealth = Core.CurrentHealth;
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        if (gameObject.CompareTag("Player"))
            Core.RestoreHealth(saveData.PlayerHealth);
    }
}
