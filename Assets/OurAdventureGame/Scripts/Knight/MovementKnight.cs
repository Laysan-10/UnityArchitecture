using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MovementKnight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraTransform; // Ссылка на камеру

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _jumpForce = 7f;

    [Header("Physics Settings")]
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheckPos;

    // Animation Hashes
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsJumpHash = Animator.StringToHash("isJump");
    private static readonly int IsGroundedHash = Animator.StringToHash("isGrounded");
    private static readonly int Attack1Hash = Animator.StringToHash("attack1");
    private static readonly int BackStepHash = Animator.StringToHash("BackStep"); // Trigger for Shift dodge if needed

    // Input Actions
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    private InputAction _attackAction;

    // State
    private Vector2 _inputVector;
    private bool _isSprinting;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        if (_cameraTransform == null && Camera.main != null)
            _cameraTransform = Camera.main.transform;

        // Настройка инпутов
        _moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        _jumpAction = new InputAction("Jump", binding: "<Keyboard>/space");
        _jumpAction.AddBinding("<Gamepad>/buttonSouth");

        _sprintAction = new InputAction("Sprint", binding: "<Keyboard>/leftShift");
        _sprintAction.AddBinding("<Gamepad>/leftTrigger");
        
        _attackAction = new InputAction("Attack", binding: "<Keyboard>/z");
        _attackAction.AddBinding("<Mouse>/leftButton");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _sprintAction.Enable();
        _attackAction.Enable();
        
        _jumpAction.performed += OnJump;
        _attackAction.performed += OnAttack;
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _sprintAction.Disable();
        _attackAction.Disable();
        
        _jumpAction.performed -= OnJump;
        _attackAction.performed -= OnAttack;
    }

    private void Update()
    {
        // Чтение инпута каждый кадр
        _inputVector = _moveAction.ReadValue<Vector2>();
        _isSprinting = _sprintAction.IsPressed();
        
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
        ApplyGravityMultiplier();
    }

    private void Move()
    {
        // 1. Рассчитываем направление относительно камеры
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        // Обнуляем Y, чтобы не идти в землю
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDir = (cameraForward * _inputVector.y + cameraRight * _inputVector.x).normalized;

        // 2. Определяем скорость
        float targetSpeed = _isSprinting ? _runSpeed : _walkSpeed;
        if (_inputVector.magnitude < 0.1f) targetSpeed = 0f;

        // 3. Применяем движение к Rigidbody (сохраняя вертикальную скорость)
        Vector3 targetVelocity = moveDir * targetSpeed;
        
        // Используем linearVelocity (Unity 6 / 2023.3+)
        Vector3 velocity = _rigidbody.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        _rigidbody.linearVelocity = velocity;

        // 4. Вращение персонажа в сторону движения
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            // Формула прыжка: sqrt(2 * gravity * height)
            float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * _jumpForce);
            Vector3 vel = _rigidbody.linearVelocity;
            vel.y = jumpVel;
            _rigidbody.linearVelocity = vel;
            
            _animator.SetTrigger(IsJumpHash);
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        _animator.SetTrigger(Attack1Hash);
    }

    private void ApplyGravityMultiplier()
    {
        // Улучшенная физика прыжка (быстрое падение)
        if (_rigidbody.linearVelocity.y < 0)
        {
            _rigidbody.linearVelocity += Vector3.up * Physics.gravity.y * (2.0f - 1) * Time.fixedDeltaTime;
        }
    }

    private void CheckGround()
    {
        // Используем сферу у ног или позицию трансформа
        Vector3 origin = _groundCheckPos ? _groundCheckPos.position : transform.position + Vector3.up * 0.1f;
        _isGrounded = Physics.CheckSphere(origin, _groundCheckRadius, _groundLayer, QueryTriggerInteraction.Ignore);
        
        _animator.SetBool(IsGroundedHash, _isGrounded);
    }
    
    private void UpdateAnimation()
    {
        // Для Blend Tree (Idle -> Walk -> Run)
        float currentSpeed = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.z).magnitude;
        _animator.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);
    }
    
    // Визуализация сферы проверки земли в редакторе
    private void OnDrawGizmosSelected()
    {
        if (_groundCheckPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheckPos.position, _groundCheckRadius);
        }
    }
}