using UnityEngine;

public class DeadState : IState {
    private EnemyBase _e;
    public DeadState(EnemyBase e) => _e = e;
    public void Enter() {
        _e.agent.enabled = false;
        _e.anim.PlayDead();
        _e.enabled = false; // Выключаем мозг
    }
    public void Update() {}
    public void Exit() {}
}
