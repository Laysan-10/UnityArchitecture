using UnityEngine;

public abstract class BaseAttackState : EnemyStateBase
{
    protected float AttackTimer;

    protected BaseAttackState(EnemyBase enemy) : base(enemy)
    {
    }

    protected abstract EnemyStateType AggressiveStateType { get; }
    protected abstract void PerformAttack();

    public override void Enter()
    {
        base.Enter();
        Enemy.agent.isStopped = true;
        Enemy.anim?.SetRunning(false);
        AttackTimer = 0f;
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

        float distance = Enemy.GetFlatDistanceToTarget();
        float combatDistance = Enemy.GetCombatDistance();

        Enemy.FaceTarget();

        AttackTimer += Time.deltaTime;
        if (AttackTimer >= Enemy.GetAttackSpeed() && Enemy.CanDamageTarget(distance))
        {
            PerformAttack();
            AttackTimer = 0f;
        }

        if (distance > Enemy.lookRadius)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Search));
            return;
        }

        if (distance > combatDistance + 0.25f)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(AggressiveStateType));
        }
    }
}
