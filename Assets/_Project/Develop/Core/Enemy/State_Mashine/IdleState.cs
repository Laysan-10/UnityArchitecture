using UnityEngine;

public class IdleState : IState
{
    private readonly EnemyBase _e;

    public IdleState(EnemyBase e) => _e = e;

    public void Enter()
    {
        _e.agent.isStopped = true;
        _e.anim?.SetRunning(false);
    }

    public void Update()
    {
        if (_e.ShouldFlee())
        {
            _e.StateMachine.ChangeState(new FleeState(_e));
            return;
        }

        if (!_e.CanAggroByProximity())
        {
            return;
        }

        if (_e.GetFlatDistanceToTarget() <= _e.lookRadius)
        {
            _e.StateMachine.ChangeState(new AggressiveState(_e));
        }
    }

    public void Exit()
    {
    }
}
