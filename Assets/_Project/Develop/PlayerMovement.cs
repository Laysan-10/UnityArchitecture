using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private Transform _cameraTransform; // Ссылка на камеру для ориентации движения

    private CharacterController _characterController;
    private IInputService _inputService;

    // Внедрение зависимости (Dependency Injection method)
    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        // Блокируем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_inputService == null) return;

        Move();
        Rotate();
    }

    private void Move()
    {
        Vector2 input = _inputService.MoveInput;
        if (input == Vector2.zero) return;

        // Определяем скорость (ходьба или бег)
        float currentSpeed = _inputService.IsSprinting ? _runSpeed : _walkSpeed;

        // Рассчитываем направление движения относительно поворота камеры
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        // Обнуляем Y, чтобы не лететь в небо/землю
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * input.y + right * input.x).normalized;

        // Движение (простая гравитация опущена для краткости, но она нужна)
        _characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector2 input = _inputService.MoveInput;
        if (input == Vector2.zero) return;
        
        // Поворачиваем модельку персонажа в сторону движения
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        
        Vector3 targetDirection = (forward * input.y + right * input.x).normalized;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}