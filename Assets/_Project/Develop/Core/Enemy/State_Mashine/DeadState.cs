public class DeadState : IState
{
    private readonly EnemyBase _e;

    public DeadState(EnemyBase e) => _e = e;

    public void Enter()
    {
        if (_e.agent != null && _e.agent.enabled)
        {
            _e.agent.enabled = false;
        }

        _e.anim?.PlayDead();
        _e.enabled = false;
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
