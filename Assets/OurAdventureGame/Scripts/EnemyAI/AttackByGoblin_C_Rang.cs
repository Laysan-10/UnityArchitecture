using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByGoblin_C_Rang : MonoBehaviour
{
    [SerializeField] private int _damageValue;
    [SerializeField] private int _damageValue2;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _target;
    [SerializeField] float attackRange = 9f;
    [SerializeField] float attackRange2 = 9f;
    public Health_Goblin_C_Rang _enemy;
    private float _distance;


    public void Damage()
    {
        if (_target != null)
        {
            _distance = Vector3.Distance(transform.position, _target.position);
            if (_distance <= attackRange)
            {
                _enemy.TakeDamageByEnemy(_damageValue);
            }
        }
    }


    public void Damage2()
    {
        if (_target != null)
        {
            _distance = Vector3.Distance(transform.position, _target.position);
            if (_distance <= attackRange2)
            {
                _enemy.TakeDamageByEnemy(_damageValue2);
            }
        }
    }
}
