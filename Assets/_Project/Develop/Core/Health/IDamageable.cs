public interface IDamageable
{
    void TakeDamage(float physicalDamage, float magicDamage);
    bool IsAlive { get; } 
}