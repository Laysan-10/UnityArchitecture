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
        _e.agent.SetDestination(_e.target.position);
        _e.anim.SetRunning(true);

        Vector3 flatTarget = _e.target.position;
        flatTarget.y = _e.transform.position.y;

        float dist = Vector3.Distance(_e.transform.position, flatTarget);
        float combatDistance = Mathf.Max(_e.attackRange, _e.agent.stoppingDistance);
        bool reachedTarget = !_e.agent.pathPending &&
            _e.agent.remainingDistance <= combatDistance + 0.05f;

        if (reachedTarget || dist <= combatDistance)
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
        _e.anim.SetRunning(false);
    }
}
