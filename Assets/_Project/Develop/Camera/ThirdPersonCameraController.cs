using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Zoom Settings")]
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
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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

        float zoomDelta = _inputService.ZoomInput;

        if (zoomDelta != 0)
        {
            _targetZoom = Mathf.Clamp(_targetZoom - zoomDelta * zoomSpeed, minDistance, maxDistance);
        }

        _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, Time.deltaTime * zoomLerpSpeed);
        _orbital.Radius = _currentZoom;
    }
}