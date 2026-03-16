using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByEnemy_C : MonoBehaviour
{
    [Header("Damage value")]
    [SerializeField] private int _damageValueC;
    [SerializeField] private int _damageValueC2;
    [SerializeField] float attackRange = 10f;
    [SerializeField] float attackRange2 = 14f;
    [SerializeField] private Animator _animatorEnemy;
    private Transform _targetEnemy;
    private HealthBarEnemy_C[] _enemies_C;
    private float _distance;

    private void Start()
    {
        GameObject targetObject = GameObject.FindWithTag("enemy_C");
        GameObject animatorObject = GameObject.FindWithTag("enemy_C");
        GameObject scriptObject = GameObject.FindWithTag("enemy_C");

        _targetEnemy = targetObject.GetComponent<Transform>();
        _animatorEnemy = animatorObject.GetComponent<Animator>();
        _enemies_C = FindObjectsOfType<HealthBarEnemy_C>();
    }

    public void Damage_C1()
    {
        foreach (var enemy in _enemies_C)
        {
            if (enemy != null)
            {
                _distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (_distance <= attackRange)
                {
                    if (enemy.HealthEnemy_C != 0)
                    {
                        enemy.TakeDamageByEnemy_C(_damageValueC);
                    }
                }
            }
        }
    }

    public void Damage_C2()
    {
        foreach (var enemy in _enemies_C)
        {
            if (enemy != null)
            {
                _distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (_distance <= attackRange2)
                {
                    if (enemy.HealthEnemy_C != 0)
                    {
                        enemy.TakeDamageByEnemy_C(_damageValueC2);
                    }
                }
            }
        }
    }

}

