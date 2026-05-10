using UnityEngine;

public class EnemyAttackSelector
{
    private readonly EnemyBase _enemy;

    public EnemyAttackSelector(EnemyBase enemy)
    {
        _enemy = enemy;
    }

    public IState GetNextAttackState()
    {
        if (_enemy.isBoss && Random.value > 0.7f)
        {
            return _enemy.CreatePowerAttackState();
        }

        return _enemy.CreateAttackState();
    }
}
