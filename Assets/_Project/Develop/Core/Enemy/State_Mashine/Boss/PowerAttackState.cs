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
        _e.anim?.SetRunning(false);
        _e.anim?.PlayPowerAttack();
    }

    public void Update()
    {
        _e.FaceTarget();

        _timer -= Time.deltaTime;

        if (!_hasDealtDamage && _timer <= _duration - _hitMoment)
        {
            _e.PerformPowerAttack();
            _hasDealtDamage = true;
        }

        if (_timer <= 0f)
        {
            _e.StateMachine.ChangeState(new AggressiveState(_e));
        }
    }

    public void Exit()
    {
    }
}
