using UnityEngine;

public class Character : IDamage
{
   private readonly IHealth health;

    public Character(IHealth health)
    {
        this.health = health;
    }

    public void TakeDamage(int amount)
    {
        health.Reduce(amount);
    }
}
