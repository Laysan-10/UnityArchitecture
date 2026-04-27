using UnityEngine;

public class PowerAttackState : IState
{
    private readonly EnemyBase _e;
    private readonly float _duration;
    private readonly float _hitMoment;
    private float _timer;
    private bool _hasDealtDamage;

    public PowerAttackState(EnemyBase e, float duration = 1.1f, float hitMoment = 0.45f)
    {
        _e = e;
        _duration = duration;
        _hitMoment = hitMoment;
    }

    public void Enter()
    {
        _timer = _duration;
        _hasDealtDamage = false;
        _e.agent.isStopped = true;
        _e.anim.SetRunning(false);
        _e.anim.PlayPowerAttack();
    }

    public void Update()
    {
        Vector3 flatTarget = _e.target.position;
        flatTarget.y = _e.transform.position.y;
        float distance = Vector3.Distance(_e.transform.position, flatTarget);

        if (!_hasDealtDamage && _timer <= _duration - _hitMoment && _e.CanDamageTarget(distance))
        {
            _e.targetDamageable.TakeDamage(_e.powerAttackDamage, 0f);
            _hasDealtDamage = true;
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _e.StateMachine.ChangeState(new AggressiveState(_e));
        }
    }

    public void Exit()
    {
    }
}
