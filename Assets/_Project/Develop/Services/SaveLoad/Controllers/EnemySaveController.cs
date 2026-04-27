using System.Collections.Generic;
using UnityEngine;

public class EnemySaveController
{
    private readonly List<EnemyAgent> _enemies;
    private readonly List<EnemySaveData> _defaultEnemyStates;

    public EnemySaveController(IEnumerable<EnemyAgent> enemies)
    {
        _enemies = new List<EnemyAgent>();
        _defaultEnemyStates = new List<EnemySaveData>();

        if (enemies == null)
        {
            return;
        }

        foreach (EnemyAgent enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            _enemies.Add(enemy);

            EnemySaveData defaultState = enemy.CaptureState();
            if (defaultState != null)
            {
                _defaultEnemyStates.Add(defaultState);
            }
        }
    }

    public List<EnemySaveData> GetEnemyData()
    {
        List<EnemySaveData> enemyStates = new List<EnemySaveData>();

        foreach (EnemyAgent enemy in _enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            EnemySaveData enemyState = enemy.CaptureState();
            if (enemyState != null)
            {
                enemyStates.Add(enemyState);
            }
        }

        return enemyStates;
    }

    public void ApplyEnemyData(IReadOnlyList<EnemySaveData> enemyStates)
    {
        if (enemyStates == null)
        {
            return;
        }

        foreach (EnemyAgent enemy in _enemies)
        {
            if (enemy != null)
            {
                enemy.RestoreState(enemyStates);
            }
        }
    }

    public void ApplyDefaultState()
    {
        ApplyEnemyData(_defaultEnemyStates);
    }
}
