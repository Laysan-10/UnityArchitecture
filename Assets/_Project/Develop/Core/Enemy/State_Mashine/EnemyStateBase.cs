using UnityEngine;

public abstract class EnemyStateBase : IState
{
    protected readonly EnemyBase Enemy;
    protected float StateEnterTime;

    protected EnemyStateBase(EnemyBase enemy)
    {
        Enemy = enemy;
    }

    public virtual void Enter()
    {
        StateEnterTime = Time.time;
    }

    public abstract void Update();

    public virtual void Exit()
    {
    }
}
