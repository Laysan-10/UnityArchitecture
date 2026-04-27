public class DeadState : EnemyStateBase
{
    public DeadState(EnemyBase enemy) : base(enemy)
    {
    }

    public override EnemyStateType StateType => EnemyStateType.Dead;

    public override void Enter()
    {
        base.Enter();

        if (Enemy.agent != null && Enemy.agent.enabled)
        {
            Enemy.agent.enabled = false;
        }

        Enemy.anim?.PlayDead();
        Enemy.enabled = false;
    }

    public override void Update()
    {
    }
}
