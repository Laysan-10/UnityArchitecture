using UnityEngine;

public class AttackState : EnemyStateBase
{
    private float _attackTimer;

    public AttackState(EnemyBase enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Enemy.agent.isStopped = true;
        Enemy.anim?.SetRunning(false);
        _attackTimer = 0f;
    }

    public override void Update()
    {
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

        float distance = Enemy.GetFlatDistanceToTarget();
        float combatDistance = Enemy.GetCombatDistance();

        Enemy.FaceTarget();

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= Enemy.GetAttackSpeed() && Enemy.CanDamageTarget(distance))
        {
            Enemy.PerformAttack();
            _attackTimer = 0f;
        }

        if (distance > Enemy.lookRadius)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateSearchState());
            return;
        }

        if (distance > combatDistance + 0.25f)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateAggressiveState());
        }
    }

    public override void Exit()
    {
    }
}
