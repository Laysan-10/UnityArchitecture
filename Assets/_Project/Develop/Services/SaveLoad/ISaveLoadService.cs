public interface ISaveLoadService
{
    void Save();
    void Load();
    void BindPlayer(PlayerMovement playerMovement, HealthComponent playerHealth);
    void ClearPlayer();
    void RegisterEnemy(newEnemyAI enemy);
    void UnregisterEnemy(newEnemyAI enemy);
    void ClearEnemies();
}
