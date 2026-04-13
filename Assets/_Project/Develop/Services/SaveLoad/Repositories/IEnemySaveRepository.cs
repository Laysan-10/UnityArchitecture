using System.Collections.Generic;

public interface IEnemySaveRepository
{
    void Register(newEnemyAI enemy);
    void Unregister(newEnemyAI enemy);
    List<EnemySaveData> GetStates();
    void Apply(IReadOnlyList<EnemySaveData> enemyStates);
    void Clear();
}
