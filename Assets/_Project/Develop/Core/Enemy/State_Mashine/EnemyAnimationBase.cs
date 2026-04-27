using UnityEngine;

public abstract class EnemyAnimationBase : MonoBehaviour
{
    public abstract void Construct(EnemyBase ai, HealthCore healthCore);
    public abstract void SetRunning(bool isRunning);
    public abstract void PlayAttack();
    public virtual void PlayPowerAttack() => PlayAttack();
    public abstract void PlayHit();
    public abstract void PlayDead();
    public abstract void ResetVisuals();
}
