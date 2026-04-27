using UnityEngine;

public class IdleState : IState {
    private EnemyBase _e;
    public IdleState(EnemyBase e) => _e = e;
    public void Enter() => _e.agent.isStopped = true;
    public void Update() {
        if (_e.isPeaceful) return;
        if (Vector3.Distance(_e.transform.position, _e.target.position) < _e.lookRadius)
            _e.StateMachine.ChangeState(new AggressiveState(_e));
    }
    public void Exit() {}
}
