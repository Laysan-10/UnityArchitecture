using UnityEngine;

public class FleeState : EnemyStateBase
{
    private float _baseSpeed;

    public FleeState(EnemyBase enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _baseSpeed = Enemy.agent.speed;
        Enemy.agent.speed = _baseSpeed * 1.5f;
        Enemy.agent.isStopped = false;
        Enemy.anim?.SetRunning(true);
    }

    public override void Update()
    {
        if (Enemy.target == null)
        {
            Enemy.StateMachine.ChangeState(new IdleState(Enemy));
            return;
        }

        Vector3 runDir = (Enemy.transform.position - Enemy.target.position).normalized;
        if (runDir.sqrMagnitude <= 0.0001f)
        {
            runDir = -Enemy.transform.forward;
        }

        Enemy.agent.SetDestination(Enemy.transform.position + runDir * 5f);

        if (!Enemy.ShouldRetreat())
        {
            if (Enemy.CanStartChase() && Enemy.GetFlatDistanceToTarget() <= Enemy.lookRadius)
            {
                Enemy.StateMachine.ChangeState(new AggressiveState(Enemy));
            }
            else
            {
                Enemy.StateMachine.ChangeState(new IdleState(Enemy));
            }
        }
    }

    public override void Exit()
    {
        Enemy.agent.speed = _baseSpeed;
        Enemy.anim?.SetRunning(false);
        Enemy.CompleteRetreat();
    }
}
