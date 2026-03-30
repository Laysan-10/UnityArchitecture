using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, ISaveable 
{
    [Header("Speed settings")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;

    private CharacterController _controller;
    private IInputService _inputService;
    private Transform _mainCameraTransform;
    private float _verticalVelocity;

    public float CurrentAnimationSpeed { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        if (Camera.main != null) _mainCameraTransform = Camera.main.transform;
    }

    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void Update()
    {
        if (Time.timeScale <= 0) return;

        if (_inputService == null || _mainCameraTransform == null) return;

        HandleMovement();
        ApplyGravity();
        if (Input.GetKeyDown(KeyCode.K)) GetComponent<IDamageable>().TakeDamage(20, 0);

    }

    private void HandleMovement()
    {
        Vector2 input = _inputService.MoveInput;

        Vector3 camForward = _mainCameraTransform.forward;
        Vector3 camRight = _mainCameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * input.y + camRight * input.x).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float currentSpeed = _inputService.IsSprinting ? sprintSpeed : walkSpeed;
            _controller.Move(moveDirection * (currentSpeed * Time.deltaTime));

            CurrentAnimationSpeed = _inputService.IsSprinting ? 0.8f : 0.4f;
        }
        else
        {
            CurrentAnimationSpeed = 0f;
        }
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _verticalVelocity < 0) _verticalVelocity = -2f;
        _verticalVelocity += gravity * Time.deltaTime;
        _controller.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }

    public void PopulateSaveData(SaveData saveData)
    {
        saveData.PlayerPosition = transform.position;
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        _controller.enabled = false;
        transform.position = saveData.PlayerPosition;
        _controller.enabled = true;
    }

    private void OnDestroy()
    {
        if (ProjectBootstrapper.Instance != null && ProjectBootstrapper.Instance.SaveLoadService != null)
        {
            ProjectBootstrapper.Instance.SaveLoadService.UnregisterSaveable(this);
        }
    }

}