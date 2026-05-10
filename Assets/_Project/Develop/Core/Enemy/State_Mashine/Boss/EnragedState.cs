using UnityEngine;

public class EnragedState : EnemyStateBase
{
    private readonly float _duration;

    public EnragedState(EnemyBase enemy, float duration = 1f) : base(enemy)
    {
        _duration = duration;
    }

    public override void Enter()
    {
        base.Enter();
        Enemy.MarkEnragedStateEntered();
        Enemy.agent.isStopped = true;
        Enemy.anim?.SetRunning(false);
        Enemy.anim?.PlayHit();
    }

    public override void Update()
    {
        Enemy.FaceTarget();

        if (Time.time - StateEnterTime < _duration)
        {
            return;
        }

        if (Enemy.target == null || !Enemy.CanStartChase())
        {
            Enemy.StateMachine.ChangeState(new IdleState(Enemy));
            return;
        }

        Enemy.StateMachine.ChangeState(new AggressiveState(Enemy));
    }
}
