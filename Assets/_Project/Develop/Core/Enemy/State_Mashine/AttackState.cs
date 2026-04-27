using UnityEngine;

public class AttackState : IState
{
    private readonly EnemyBase _e;
    private float _timer;

    public AttackState(EnemyBase e) => _e = e;

    public void Enter()
    {
        _e.agent.isStopped = true;
        _e.anim.SetRunning(false);
        _timer = 0f;
    }

    public void Update()
    {
        float combatDistance = Mathf.Max(_e.attackRange, _e.agent.stoppingDistance);

        Vector3 flatTarget = _e.target.position;
        flatTarget.y = _e.transform.position.y;
        float distance = Vector3.Distance(_e.transform.position, flatTarget);

        _timer += Time.deltaTime;
        if (_timer >= _e.GetAttackSpeed() && _e.CanDamageTarget(distance))
        {
            if (_e.isBoss && Random.value > 0.7f)
            {
                _e.StateMachine.ChangeState(new PowerAttackState(_e));
                return;
            }

            _e.anim.PlayAttack();
            _e.targetDamageable.TakeDamage(_e.attackDamage, 0f);
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
