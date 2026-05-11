using System.Collections.Generic;
using UnityEngine;

public class EnemySaveController
{
    private readonly List<EnemyBase> _enemies;
    private readonly List<EnemySaveData> _defaultEnemyStates;

    public EnemySaveController(IEnumerable<EnemyBase> enemies)
    {
        _enemies = new List<EnemyBase>();
        _defaultEnemyStates = new List<EnemySaveData>();

        if (enemies == null)
        {
            return;
        }

        foreach (EnemyBase enemy in enemies)
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

        foreach (EnemyBase enemy in _enemies)
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

        foreach (EnemyBase enemy in _enemies)
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
