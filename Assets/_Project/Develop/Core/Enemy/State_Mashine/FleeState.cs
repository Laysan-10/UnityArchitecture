using UnityEngine;

public class FleeState : IState
{
    private readonly EnemyBase _e;
    private float _baseSpeed;

    public FleeState(EnemyBase e) => _e = e;

    public void Enter()
    {
        _baseSpeed = _e.agent.speed;
        _e.agent.speed = _baseSpeed * 1.5f;
        _e.agent.isStopped = false;
        _e.anim?.SetRunning(true);
    }

    public void Update()
    {
        if (_e.target == null)
        {
            _e.StateMachine.ChangeState(new IdleState(_e));
            return;
        }

        Vector3 runDir = (_e.transform.position - _e.target.position).normalized;
        if (runDir.sqrMagnitude <= 0.0001f)
        {
            runDir = -_e.transform.forward;
        }

        _e.agent.SetDestination(_e.transform.position + runDir * 5f);

        if (!_e.ShouldFlee())
        {
            if (_e.CanAggroByProximity() && _e.GetFlatDistanceToTarget() <= _e.lookRadius)
            {
                _e.StateMachine.ChangeState(new AggressiveState(_e));
            }
            else
            {
                _e.StateMachine.ChangeState(new IdleState(_e));
            }
        }
    }

    public void Exit()
    {
        _e.agent.speed = _baseSpeed;
        _e.anim?.SetRunning(false);
    }
}
