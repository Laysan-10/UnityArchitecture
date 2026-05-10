using UnityEngine;

public class PowerAttackState : EnemyStateBase
{
    private readonly float _duration;
    private readonly float _hitMoment;
    private bool _hasDealtDamage;
    private float _attackTimer;

    public PowerAttackState(EnemyBase enemy, float duration = 1.1f, float hitMoment = 0.45f) : base(enemy)
    {
        _duration = duration;
        _hitMoment = hitMoment;
    }

    public override void Enter()
    {
        base.Enter();
        Enemy.agent.isStopped = true;
        Enemy.anim?.SetRunning(false);
        _hasDealtDamage = false;
        _attackTimer = 0f;
        Enemy.anim?.PlayPowerAttack();
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

        Enemy.FaceTarget();
        _attackTimer += Time.deltaTime;

        if (!_hasDealtDamage && _attackTimer >= _hitMoment)
        {
            Enemy.PerformPowerAttack();
            _hasDealtDamage = true;
        }

        if (Enemy.GetFlatDistanceToTarget() > Enemy.lookRadius)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateSearchState());
            return;
        }

        if (_attackTimer >= _duration)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateAggressiveState());
        }
    }

    public override void Exit()
    {
    }
}
