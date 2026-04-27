using UnityEngine;

public class SearchState : EnemyStateBase
{
    private readonly float _searchDuration;

    public SearchState(EnemyBase enemy, float searchDuration = 2f) : base(enemy)
    {
        _searchDuration = searchDuration;
    }

    public override EnemyStateType StateType => EnemyStateType.Search;

    public override void Enter()
    {
        base.Enter();
        Enemy.agent.isStopped = false;
        Enemy.anim?.SetRunning(true);
        Enemy.agent.SetDestination(Enemy.TrackedTargetPosition);
    }

    public override void Update()
    {
        if (Enemy.ShouldAbortCombat())
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Idle));
            return;
        }

        if (Enemy.ShouldEnterEnragedState())
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Enraged));
            return;
        }

        if (Enemy.ShouldRetreat())
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Flee));
            return;
        }

        if (Enemy.CanStartChase() && Enemy.GetFlatDistanceToTarget() <= Enemy.lookRadius)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Aggressive));
            return;
        }

        if (Time.time - StateEnterTime >= _searchDuration || Enemy.agent.remainingDistance <= Enemy.agent.stoppingDistance + 0.1f)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Idle));
        }
    }

    public override void Exit()
    {
        Enemy.anim?.SetRunning(false);
    }
}
