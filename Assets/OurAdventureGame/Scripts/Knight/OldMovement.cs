using UnityEngine;
using System.Collections;

public class OldMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _speed = 8;

    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float fallMultiplier = 3.0f; // Множитель для ускорения падения
    [SerializeField] private float lowJumpMultiplier = 2.5f; // Множитель для плавного падения

    const string RUN = "isRun";
    const string JUMP = "isJump";
    const string ATTACK1 = "attack1";
    const string ATTACK2 = "attack2";

    [SerializeField] private bool isRun;
    [SerializeField] private bool isJump;
    private bool isFacingRight = false; // Переменная для отслеживания ориентации персонажа


    [SerializeField] private float _rotationBack = 90f; // Угол поворота персонажа назад
    [SerializeField] private float _rotationDown = -90f; // Угол поворота персонажа вперед


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();


    }

    private void Update()
    {
        Move();
    }



    protected void Move()
    {
        isRun = false;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(0, 0, 1 * Time.deltaTime * _speed));
            _animator.SetBool(RUN, true);
            isRun = true;

            if (!isFacingRight) // Если персонаж не смотрит вправо
            {
                RotateCharacter(_rotationDown); // Поворот вперед
                isFacingRight = true; // Отметим, что персонаж смотрит вправо
                _speed = Mathf.Abs(_speed);
            }
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(0, 0, -1 * Time.deltaTime * _speed));
            _animator.SetBool(RUN, true);
            isRun = true;

            if (isFacingRight) // Если персонаж смотрит вправо
            {
                RotateCharacter(_rotationBack); // Поворот назад
                isFacingRight = false; // Отметим, что персонаж смотрит влево
                _speed = -Mathf.Abs(_speed);
            }
        }
        if (!isRun)
        {
            _animator.SetBool(RUN, false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isJump)
        {
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, CalculateJumpForce(), _rigidbody.linearVelocity.z); // Изменение силы прыжка
            isJump = false;
            _animator.SetBool(JUMP, true);
        }

        // Применение ускоренного падения
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            _animator.SetTrigger(ATTACK2);

        }
    }

    void RotateCharacter(float angle)
    {
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private float CalculateJumpForce()
    {
        // Используйте формулу для расчета скорости при прыжке с учетом гравитации
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


