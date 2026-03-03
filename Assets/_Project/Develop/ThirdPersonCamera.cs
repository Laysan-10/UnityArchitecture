using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform _target; // Игрок
    [SerializeField] private float _sensitivity = 2f;
    [SerializeField] private float _distance = 5f;
    [SerializeField] private Vector2 _verticalLimits = new Vector2(-40, 80);

    private IInputService _inputService;
    private float _rotationX;
    private float _rotationY;

    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void LateUpdate()
    {
        if (_inputService == null || _target == null) return;

        Vector2 lookInput = _inputService.LookInput;

        _rotationY += lookInput.x * _sensitivity;
        _rotationX -= lookInput.y * _sensitivity;
        _rotationX = Mathf.Clamp(_rotationX, _verticalLimits.x, _verticalLimits.y);

        Quaternion rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        Vector3 position = rotation * new Vector3(0, 0, -_distance) + _target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

}