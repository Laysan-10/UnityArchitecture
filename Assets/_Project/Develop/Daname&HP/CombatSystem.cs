using UnityEngine;

public class CombatSystem
{
    public void ApplyDamage(IDamage target, IDamageSource source)
    {
        target.TakeDamage(source.GetDamage());
    }
}
