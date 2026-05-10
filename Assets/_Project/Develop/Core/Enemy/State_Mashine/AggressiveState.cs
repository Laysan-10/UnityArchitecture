using UnityEngine;

public class AggressiveState : EnemyStateBase
{
    public AggressiveState(EnemyBase enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        if (Enemy.target == null)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateIdleState());
            return;
        }

        if (Enemy.ShouldAbortCombat())
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateIdleState());
            return;
        }

        if (Enemy.ShouldEnterEnragedState())
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateEnragedState());
            return;
        }

        if (Enemy.ShouldRetreat())
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateFleeState());
            return;
        }

        Enemy.agent.SetDestination(Enemy.target.position);
        Enemy.anim?.SetRunning(true);

        float dist = Enemy.GetFlatDistanceToTarget();
        bool reachedTarget = !Enemy.agent.pathPending &&
            Enemy.agent.remainingDistance <= Enemy.GetCombatDistance() + 0.05f;

        if (reachedTarget || dist <= Enemy.GetCombatDistance())
        {
            Enemy.StateMachine.ChangeState(Enemy.ChooseAttackState());
            return;
        }

        if (dist > Enemy.lookRadius)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateSearchState());
        }
    }

    public override void Exit()
    {
        Enemy.anim?.SetRunning(false);
    }
}
