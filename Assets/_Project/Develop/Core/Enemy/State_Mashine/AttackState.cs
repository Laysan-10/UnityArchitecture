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
            Enemy.StateMachine.ChangeState(new IdleState(Enemy));
            return;
        }

        if (Enemy.ShouldEnterEnragedState())
        {
            Enemy.StateMachine.ChangeState(new EnragedState(Enemy));
            return;
        }

        if (Enemy.ShouldRetreat())
        {
            Enemy.StateMachine.ChangeState(new FleeState(Enemy));
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
            Enemy.StateMachine.ChangeState(new SearchState(Enemy));
            return;
        }

        if (distance > combatDistance + 0.25f)
        {
            Enemy.StateMachine.ChangeState(new AggressiveState(Enemy));
        }
    }

    public override void Exit()
    {
    }
}
