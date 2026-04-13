public interface IPlayerSaveRepository
{
    void Bind(PlayerMovement playerMovement, HealthComponent playerHealth);
    void Clear();
    PlayerSaveData GetState();
    void Apply(PlayerSaveData playerState);
}
