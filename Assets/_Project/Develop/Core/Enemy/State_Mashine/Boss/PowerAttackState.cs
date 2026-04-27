using UnityEngine;

public class PowerAttackState : BaseAttackState
{
    private readonly float _duration;
    private readonly float _hitMoment;
    private bool _hasDealtDamage;

    public PowerAttackState(EnemyBase enemy, float duration = 1.1f, float hitMoment = 0.45f) : base(enemy)
    {
        _duration = duration;
        _hitMoment = hitMoment;
    }

    public override EnemyStateType StateType => EnemyStateType.PowerAttack;
    protected override EnemyStateType AggressiveStateType => EnemyStateType.Aggressive;

    public override void Enter()
    {
        base.Enter();
        _hasDealtDamage = false;
        Enemy.anim?.PlayPowerAttack();
    }

    public override void Update()
    {
        if (Enemy.ShouldRetreat())
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Flee));
            return;
        }

        Enemy.FaceTarget();
        AttackTimer += Time.deltaTime;

        if (!_hasDealtDamage && AttackTimer >= _hitMoment)
        {
            PerformAttack();
            _hasDealtDamage = true;
        }

        if (Enemy.GetFlatDistanceToTarget() > Enemy.lookRadius)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Search));
            return;
        }

        if (AttackTimer >= _duration)
        {
            Enemy.StateMachine.ChangeState(Enemy.CreateState(EnemyStateType.Aggressive));
        }
    }

    protected override void PerformAttack()
    {
        Enemy.PerformPowerAttack();
    }
}
