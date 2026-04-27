using UnityEngine;

public class StunState : IState {
    private EnemyBase _e;
    private float _duration = 2f;
    public StunState(EnemyBase e) => _e = e;
    public void Enter() { _e.agent.isStopped = true; _e.anim.PlayHit(); }
    public void Update() {
        _duration -= Time.deltaTime;
        if (_duration <= 0) _e.StateMachine.ChangeState(new AggressiveState(_e));
    }
    public void Exit() {}
}