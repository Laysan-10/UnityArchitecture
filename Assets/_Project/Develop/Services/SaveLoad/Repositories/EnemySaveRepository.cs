using System.Collections.Generic;

public class EnemySaveRepository : IEnemySaveRepository
{
    private readonly List<newEnemyAI> _enemies = new();

    public void Register(newEnemyAI enemy)
    {
        if (enemy != null && !_enemies.Contains(enemy))
        {
            _enemies.Add(enemy);
        }
    }

    public void Unregister(newEnemyAI enemy)
    {
        _enemies.Remove(enemy);
    }

    public List<EnemySaveData> GetStates()
    {
        List<EnemySaveData> enemyStates = new();

        foreach (newEnemyAI enemy in _enemies)
        {
            EnemySaveData enemyState = enemy.CaptureState();
            if (enemyState != null)
            {
                enemyStates.Add(enemyState);
            }
        }

        return enemyStates;
    }

    public void Apply(IReadOnlyList<EnemySaveData> enemyStates)
    {
        foreach (newEnemyAI enemy in _enemies)
        {
            enemy.RestoreState(enemyStates);
        }
    }

    public void Clear()
    {
        _enemies.Clear();
    }
}
