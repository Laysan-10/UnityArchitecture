using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки скорости")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float gravity = -9.81f;

    private CharacterController _controller;
    private IInputService _inputService;
    private float _verticalVelocity;

    // Публичное свойство для Аниматора (0 - стоит, 0.4 - идет, 0.8 - бежит)
    public float CurrentAnimationSpeed { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Внедрение зависимости (Dependency Injection)
    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void Update()
    {
        if (_inputService == null) return;

        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        Vector2 input = _inputService.MoveInput;
        Vector3 moveDirection = new Vector3(input.x, 0f, input.y).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            // Поворот персонажа по направлению движения
            transform.forward = moveDirection;

            // Выбор скорости
            float currentSpeed = _inputService.IsSprinting ? sprintSpeed : walkSpeed;
            _controller.Move(moveDirection * (currentSpeed * Time.deltaTime));

            // Передаем точные значения для вашего BlendTree (0.4 - walk, 0.8 - run)
            CurrentAnimationSpeed = _inputService.IsSprinting ? 0.8f : 0.4f;
        }
        else
        {
            CurrentAnimationSpeed = 0f;
        }
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f; // Прижимаем к земле
        }

        _verticalVelocity += gravity * Time.deltaTime;
        _controller.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }
}