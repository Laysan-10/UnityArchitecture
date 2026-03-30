using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dark_magic_ball : MonoBehaviour
{
    public Player _player;
    public Transform _transformEnemy;

    [SerializeField] private ParticleSystem _dark_magic_ball;

    [SerializeField] Animator _animator;
    private string attack = "IsAttack";

    public Transform hitPosition; // HitPosition
    [SerializeField] Transform posMove; //  PosMove

    [SerializeField] private float cooldownTime = 7f;
    private bool isOnCooldown = false;
    [SerializeField] private float LookRadius;

    [SerializeField] private int damageAmount;
    private bool isDamageApplied = false; //

    [SerializeField] private ParticleSystem _shieldActive;


    private void Update()
    {
        float distance = Vector3.Distance(transform.position, _transformEnemy.position);
        if (distance < LookRadius && isOnCooldown == false)
        {
            StartCoroutine(MagicAttack());
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (_shieldActive.isPlaying)
        {
            Debug.Log("Shield is active");
            return;
        }

        Debug.Log("collision");
        if (other.CompareTag("MagicBall") && !isDamageApplied)
        {
            Debug.Log("Collision with particle systemm");
            _player.TakeDamage(damageAmount);
            isDamageApplied = true;
        }
    }

    public void PlayEffect()
    {
        //PosMove  HitPosition
        posMove.position = hitPosition.position;

        ParticleSystem darkMagicBall = Instantiate(_dark_magic_ball, posMove.position, Quaternion.identity);
        darkMagicBall.transform.parent = null;
        darkMagicBall.Play();
        Destroy(darkMagicBall.gameObject, 5f);
    }

    private IEnumerator MagicAttack()
    {
        isOnCooldown = true;
        isDamageApplied = false;
        _animator.SetTrigger(attack);
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}
