using UnityEngine;

public class PlayerSaveRepository : IPlayerSaveRepository
{
    private PlayerMovement _playerMovement;
    private HealthComponent _playerHealth;

    public void Bind(PlayerMovement playerMovement, HealthComponent playerHealth)
    {
        _playerMovement = playerMovement;
        _playerHealth = playerHealth;
    }

    public void Clear()
    {
        _playerMovement = null;
        _playerHealth = null;
    }

    public PlayerSaveData GetState()
    {
        if (_playerMovement == null || _playerHealth == null)
        {
            Debug.LogWarning("Player repository is not bound to scene objects.");
            return new PlayerSaveData();
        }

        return new PlayerSaveData
        {
            Position = _playerMovement.CapturePosition(),
            CurrentHealth = _playerHealth.CurrentHealth
        };
    }

    public void Apply(PlayerSaveData playerState)
    {
        if (playerState == null || _playerMovement == null || _playerHealth == null)
        {
            return;
        }

        _playerMovement.RestorePosition(playerState.Position);
        _playerHealth.RestoreHealth(playerState.CurrentHealth);
    }
}
