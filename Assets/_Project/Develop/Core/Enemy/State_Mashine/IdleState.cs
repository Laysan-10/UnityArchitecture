using UnityEngine;

public class IdleState : EnemyStateBase
{
    public IdleState(EnemyBase enemy) : base(enemy)
    {
    }

    public override EnemyStateType StateType => EnemyStateType.Idle;

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
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Flee));
            return;
        }

        if (Enemy.ShouldEnterEnragedState())
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Enraged));
            return;
        }

        if (!Enemy.CanStartChase())
        {
            return;
        }

        if (Enemy.GetFlatDistanceToTarget() <= Enemy.lookRadius)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Aggressive));
        }
    }
}
