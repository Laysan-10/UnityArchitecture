using UnityEngine;

public class AggressiveState : IState {
    private EnemyBase _e;
    public AggressiveState(EnemyBase e) => _e = e;
    public void Enter() => _e.agent.isStopped = false;
    public void Update() {
        _e.agent.SetDestination(_e.target.position);
        _e.anim.SetRunning(true); // Нужно добавить метод SetRunning в контроллер

        float dist = Vector3.Distance(_e.transform.position, _e.target.position);
        if (dist <= _e.attackRange) _e.StateMachine.ChangeState(new AttackState(_e));
        if (dist > _e.lookRadius) _e.StateMachine.ChangeState(new IdleState(_e));
    }
    public void Exit() => _e.anim.SetRunning(false);
}