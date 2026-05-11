using UnityEngine;

public class PlayerSaveController
{
    private readonly PlayerMovement _playerMovement;
    private readonly HealthComponent _playerHealth;
    private readonly Vector3 _defaultPosition;
    private readonly float _defaultHealth;

    public PlayerSaveController(PlayerMovement playerMovement, HealthComponent playerHealth)
    {
        _playerMovement = playerMovement;
        _playerHealth = playerHealth;
        _defaultPosition = playerMovement != null ? playerMovement.CapturePosition() : Vector3.zero;
        _defaultHealth = playerHealth != null ? playerHealth.CurrentHealth : 100f;
    }

    public PlayerSaveData GetPlayerData()
    {
        if (_playerMovement == null || _playerHealth == null)
        {
            Debug.LogWarning("PlayerSaveController is not configured.");
            return new PlayerSaveData();
        }

        return new PlayerSaveData
        {
            Position = _playerMovement.CapturePosition(),
            CurrentHealth = _playerHealth.CurrentHealth
        };
    }

    public void ApplyPlayerData(PlayerSaveData playerData)
    {
        if (playerData == null || _playerMovement == null || _playerHealth == null)
        {
            return;
        }

        _playerMovement.RestorePosition(playerData.Position);
        _playerHealth.RestoreHealth(playerData.CurrentHealth);
    }

    public void ApplyDefaultState()
    {
        ApplyPlayerData(new PlayerSaveData
        {
            Position = _defaultPosition,
            CurrentHealth = _defaultHealth
        });
    }
}
