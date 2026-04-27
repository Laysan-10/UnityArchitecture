using UnityEngine;

public class FleeState : IState {
    private EnemyBase _e;
    public FleeState(EnemyBase e) => _e = e;
    public void Enter() => _e.agent.speed *= 1.5f;
    public void Update() {
        Vector3 runDir = (_e.transform.position - _e.target.position).normalized;
        _e.agent.SetDestination(_e.transform.position + runDir * 5f);
    }
    public void Exit() => _e.agent.speed /= 1.5f;
}
