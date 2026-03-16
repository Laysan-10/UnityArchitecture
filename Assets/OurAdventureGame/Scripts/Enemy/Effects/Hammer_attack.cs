using System.Collections;
using UnityEngine;

public class Hammer_attack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] ParticleSystem _hammer_effect;
    [SerializeField] Animator _animator;
    public Player  _player;
    [SerializeField] float attackRange = 2f;  // Расстояние, на котором атака считается успешной
    private float distance;

    public void PlayEffect()
    {
        _hammer_effect.Play();
        Debug.Log("Attack Goblin S Rang");
    }

    public void Damage()
    {
        if (_player != null)
        {
            distance = Vector3.Distance(transform.position, _player.transform.position);

            // Проверяем, находится ли игрок в пределах допустимого расстояния для атаки
            if (distance <= attackRange)
            {
                _player.TakeDamage(15);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
