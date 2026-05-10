using UnityEngine;

public class IdleState : EnemyStateBase
{
    public IdleState(EnemyBase enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Enemy.agent.isStopped = true;
        Enemy.anim?.SetRunning(false);
    }

    public override void Update()
    {
        if (Enemy.ShouldRetreat())
        {
            Enemy.StateMachine.ChangeState(new FleeState(Enemy));
            return;
        }

        if (Enemy.ShouldEnterEnragedState())
        {
            Enemy.StateMachine.ChangeState(new EnragedState(Enemy));
            return;
        }

        if (!Enemy.CanStartChase())
        {
            return;
        }

        if (Enemy.GetFlatDistanceToTarget() <= Enemy.lookRadius)
        {
            Enemy.StateMachine.ChangeState(new AggressiveState(Enemy));
        }
    }
}
