using UnityEngine;

public class PowerAttackState : IState {
    private EnemyBase _e;
    public PowerAttackState(EnemyBase e) => _e = e;
    public void Enter() { _e.anim.PlayPowerAttack(); } // Нужно добавить в аниматор
    public void Update() {
        // После удара возвращаемся в агрессию
        _e.StateMachine.ChangeState(new AggressiveState(_e));
    }
    public void Exit() {}
}
