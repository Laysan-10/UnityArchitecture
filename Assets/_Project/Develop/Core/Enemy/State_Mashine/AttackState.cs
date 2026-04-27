using UnityEngine;

public class AttackState : IState
{
    private readonly EnemyBase _e;
    private float _timer;

    public AttackState(EnemyBase e) => _e = e;

    public void Enter()
    {
        _e.agent.isStopped = true;
        _e.anim?.SetRunning(false);
        _timer = 0f;
    }

    public void Update()
    {
        if (_e.ShouldFlee())
        {
            _e.StateMachine.ChangeState(new FleeState(_e));
            return;
        }

        float distance = _e.GetFlatDistanceToTarget();
        float combatDistance = _e.GetCombatDistance();

        _e.FaceTarget();

        _timer += Time.deltaTime;
        if (_timer >= _e.GetAttackSpeed() && _e.CanDamageTarget(distance))
        {
            if (_e.isBoss && Random.value > 0.7f)
            {
                _e.StateMachine.ChangeState(new PowerAttackState(_e));
                return;
            }

            _e.PerformAttack();
            _timer = 0f;
        }

        if (distance > combatDistance + 0.25f)
        {
            _e.StateMachine.ChangeState(new AggressiveState(_e));
        }
    }

    public void Exit()
    {
    }
}
