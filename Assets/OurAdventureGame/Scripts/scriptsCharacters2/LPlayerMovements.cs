using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPlayerMovements : MonoBehaviour
{
    

    [SerializeField] private float _speedWalk;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpPower;
    private bool _isJump;

    private BarbarianAnimator barbarianAnimator;

    private CharacterController _characterController;
    private Vector3 _walkDirection;
    private Vector3 _velocity;


    private void Start()
    {
        transform.Rotate(0, 0, 0);
        _characterController = GetComponent<CharacterController>();
        barbarianAnimator = GetComponentInChildren<BarbarianAnimator>();
        
    }

    private void Update()
    {

        Jump(Input.GetKey(KeyCode.Space) && _characterController.isGrounded);
        
        
        
        float y = Input.GetAxis("Horizontal");
        _walkDirection = transform.right * y;
    }
    private void FixedUpdate()
    {
        Walk(_walkDirection);
        DoGravity(_characterController.isGrounded);
    }
    private void Walk(Vector3 direction)
    {
        _characterController.Move(direction * _speedWalk * Time.fixedDeltaTime);
      

    }

    public void DoGravity(bool isGrounded)
    {
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -1f;
           
        }
        _velocity.y -= _gravity * Time.fixedDeltaTime;
        _characterController.Move(_velocity * Time.fixedDeltaTime);
    }
    private void Jump(bool canjump)
    {
        if (canjump)
        {
            _velocity.y = _jumpPower;
    
            barbarianAnimator.Jumping();
            



        }
        if (_characterController.isGrounded && Input.GetKey(KeyCode.Space)!= true)
        {
             barbarianAnimator.JumpingStop();
        }


    }
}
