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

        Enemy.FaceTarget();
        _attackTimer += Time.deltaTime;

        if (!_hasDealtDamage && _attackTimer >= _hitMoment)
        {
            Enemy.PerformPowerAttack();
            _hasDealtDamage = true;
        }

        if (Enemy.GetFlatDistanceToTarget() > Enemy.lookRadius)
        {
            Enemy.StateMachine.ChangeState(new SearchState(Enemy));
            return;
        }

        if (_attackTimer >= _duration)
        {
            Enemy.StateMachine.ChangeState(new AggressiveState(Enemy));
        }
    }

    public override void Exit()
    {
    }
}
