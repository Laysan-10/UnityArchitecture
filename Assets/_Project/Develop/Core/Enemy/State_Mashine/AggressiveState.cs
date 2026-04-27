using UnityEngine;

public class AggressiveState : IState
{
    private readonly EnemyBase _e;

    public AggressiveState(EnemyBase e) => _e = e;

    public void Enter()
    {
        _e.agent.isStopped = false;
    }

    public void Update()
    {
        if (_e.target == null)
        {
            _e.StateMachine.ChangeState(new IdleState(_e));
            return;
        }

        if (_e.ShouldFlee())
        {
            _e.StateMachine.ChangeState(new FleeState(_e));
            return;
        }

        _e.agent.SetDestination(_e.target.position);
        _e.anim?.SetRunning(true);

        float dist = _e.GetFlatDistanceToTarget();
        bool reachedTarget = !_e.agent.pathPending &&
            _e.agent.remainingDistance <= _e.GetCombatDistance() + 0.05f;

        if (reachedTarget || dist <= _e.GetCombatDistance())
        {
            _e.StateMachine.ChangeState(new AttackState(_e));
            return;
        }

        if (dist > _e.lookRadius)
        {
            _e.StateMachine.ChangeState(new IdleState(_e));
        }
    }

    public void Exit()
    {
        _e.anim?.SetRunning(false);
    }
}
