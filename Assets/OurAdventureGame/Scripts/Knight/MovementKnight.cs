using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MovementKnight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Movement Parameters")]
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float fallMultiplier = 3.0f;
    [SerializeField] private float lowJumpMultiplier = 2.5f;

    [Header("Animation States")]
    const string RUN = "isRun";
    const string JUMP = "isJump";
    const string ATTACK1 = "attack1";
    //const string ATTACK2 = "attack2";

    [Header("State Flags")]
    private bool isRun;
    private bool isJump;
    private bool isFacingRight = true;

    [Header("Rotation Parameters")]
    [SerializeField] private float _rotationBack = 90f;
    [SerializeField] private float _rotationDown = -90f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();

    }

    protected void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0);
        movement.Normalize();
        _rigidbody.AddForce(movement * _speed, ForceMode.VelocityChange);

        Vector3 velocity = new Vector3(movement.x * _speed, _rigidbody.linearVelocity.y, 0);
        _rigidbody.linearVelocity = velocity;
        isRun = Mathf.Abs(horizontalInput) > 0;

        if (isRun)
        {
            if (horizontalInput > 0 && !isFacingRight)
            {
                RotateCharacter(_rotationDown);
                isFacingRight = true;
            }
            else if (horizontalInput < 0 && isFacingRight)
            {
                RotateCharacter(_rotationBack);
                isFacingRight = false;
            }
        }

        _animator.SetBool(RUN, isRun);

        if (Input.GetKeyDown(KeyCode.Space) && isJump)
        {
            _rigidbody.linearVelocity = new Vector3(velocity.x, CalculateJumpForce(), velocity.z);
            isJump = false;
            _animator.SetBool(JUMP, true);
        }

        if (_rigidbody.linearVelocity.y < 0)
        {
            _rigidbody.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rigidbody.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            _rigidbody.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _animator.SetTrigger("BackStep");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _animator.SetTrigger(ATTACK1);
        }

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    _animator.SetTrigger(ATTACK2);
        //}
    }

    void RotateCharacter(float angle)
    {
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private float CalculateJumpForce()
    {
        return Mathf.Sqrt(2 * jumpForce * Mathf.Abs(Physics.gravity.y));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isJump = true;
            _animator.SetBool("isJump", false);
        }
    }
}