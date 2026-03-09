using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Настройки зума")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomLerpSpeed = 10f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;

    private IInputService _inputService;
    private CinemachineCamera _cam;
    private CinemachineOrbitalFollow _orbital;
    
    private float _targetZoom;
    private float _currentZoom;

    private void Awake()
    {
        _cam = GetComponent<CinemachineCamera>();
        _orbital = GetComponent<CinemachineOrbitalFollow>();
        
        // Прячем и блокируем курсор мыши
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Внедрение зависимостей (DIP)
    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
        
        if (_orbital != null)
        {
            _targetZoom = _currentZoom = _orbital.Radius;
        }
    }

    private void Update()
    {
        if (_inputService == null || _orbital == null) return;

        // Читаем ввод зума из сервиса
        float zoomDelta = _inputService.ZoomInput;

        if (zoomDelta != 0)
        {
            // Отнимаем delta, чтобы при скролле вверх (положительное значение) камера приближалась (радиус уменьшался)
            _targetZoom = Mathf.Clamp(_targetZoom - zoomDelta * zoomSpeed, minDistance, maxDistance);
        }

        // Плавное изменение радиуса орбиты
        _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, Time.deltaTime * zoomLerpSpeed);
        _orbital.Radius = _currentZoom;
    }
}