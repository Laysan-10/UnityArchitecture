using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private ThirdPersonCamera _cameraController;

    private InputService _inputService;

    private void Awake()
    {
        _inputService = new InputService();

        _playerMovement.Construct(_inputService);
        _cameraController.Construct(_inputService);
    }
    
    private void OnDestroy()
    {
        _inputService.Cleanup();
    }
}