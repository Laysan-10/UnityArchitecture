using UnityEngine;

public class AttackState : IState {
    private EnemyBase _e;
    private float _timer;
    public AttackState(EnemyBase e) => _e = e;
    public void Enter() { _e.agent.isStopped = true; _timer = 0; }
    public void Update() {
        _timer += Time.deltaTime;
        if (_timer >= _e.GetAttackSpeed()) {
            _e.anim.PlayAttack();
            _timer = 0;
            // Если это босс, есть шанс перейти в сильную атаку
            if (_e.isBoss && Random.value > 0.7f) _e.StateMachine.ChangeState(new PowerAttackState(_e));
        }
        if (Vector3.Distance(_e.transform.position, _e.target.position) > _e.attackRange)
            _e.StateMachine.ChangeState(new AggressiveState(_e));
    }
    public void Exit() {}
}
