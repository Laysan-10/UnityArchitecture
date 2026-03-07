using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform modelRoot;
    
    [Header("Components")]
    private PlayerMovement _movement;
    private PlayerAnimationController _animationController;
    private InputService _inputService;
    
    [Header("Camera")]
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float yRotation;
    
    
    public PlayerMovement Movement => _movement;
    public InputService InputService => _inputService;
    public Transform AimTarget => aimTarget;
    public PlayerCameraController CameraController => cameraController;
    public Camera MainCamera => _mainCamera;

    private Camera _mainCamera;
    
    private void Awake()
    {
        InitializeComponents();
        _mainCamera = Camera.main;
    }
    
    private void Start()
    {
        InitializeSystems();
        // _inputService.OnPhysicalAttackPressed += animationController.PlayPhysicalAttack;
        // _inputService.OnMagicAttackPressed += animationController.PlayMagicAttack;
    }
    
    private void Update()
    {
        Debug.Log("PlayerController Update");

        //AimTargetFollow();
        HandleInput();
    }
    
    private void InitializeComponents()
    {
        _movement = GetComponent<PlayerMovement>();
        _animationController = GetComponent<PlayerAnimationController>();
        _inputService = GetComponent<InputService>();
        
        if (!cameraController)
        {
            cameraController = GetComponent<PlayerCameraController>();
            if (!cameraController)
            {
                cameraController = gameObject.AddComponent<PlayerCameraController>();
            }
        }
    }

    private void InitializeStats()
    {
        
        _movement.SetMoveSpeed(5f);
    }


    private void InitializeSystems()
    {
        InitializeStats();
    }
    
        
    private void HandleInput()
    {
        if (!_inputService)
        {
            Debug.Log("InputService NULL");
            return;
        }

        Debug.Log("Controller MoveInput: " + _inputService.MoveInput);

        _movement.SetMoveInput(_inputService.MoveInput);
        _movement.SetSprintInput(_inputService.SprintPressed);
        
        if (cameraController)
        {
            Transform orientation = cameraController.GetOrientation();
            if (orientation)
            {
                _movement.SetOrientation(orientation);
            }
            
            Transform camTarget = cameraController.GetCameraFollowTarget();
            if (camTarget && _mainCamera)
            {
                _movement.SetCameraTransform(_mainCamera.transform);
            }
        }
    }

    private void LateUpdate()
    {
        UpdateModelRotation();
    }

    private void UpdateModelRotation()
    {
        if (!modelRoot || !cameraController)
        {
            return;
        }

        Transform orientation = cameraController.GetOrientation();
        if (!orientation)
        {
            return;
        }

        Vector3 euler = orientation.rotation.eulerAngles;
        euler.x = 0f;
        euler.z = 0f;
        euler.y += yRotation;
        modelRoot.rotation = Quaternion.Euler(euler);
    }
    
        
    private void AimTargetFollow()
    {
        Ray desiredTargetRay = _mainCamera.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
        Vector3 desiredTargetPos = desiredTargetRay.origin + desiredTargetRay.direction * 0.7f;
        aimTarget.position = desiredTargetPos;
    }
}

