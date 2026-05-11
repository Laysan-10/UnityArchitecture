using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByEnemy_A : MonoBehaviour
{
    [Header("Damage value")]
    [SerializeField] private int _damageValueA;
    [SerializeField] private int _damageValueA2;
    [SerializeField] float attackRange = 10f;
    [SerializeField] float attackRange2 = 14f;
    [SerializeField] private Animator _animatorEnemy;
    private Transform _targetEnemy;
    private HealthBarEnemy_A[] _enemies_A;
    private float _distance;

    private void Start()
    {
        GameObject targetObject = GameObject.FindWithTag("enemy_A");
        GameObject animatorObject = GameObject.FindWithTag("enemy_A");
        GameObject scriptObject = GameObject.FindWithTag("enemy_A");

        _targetEnemy = targetObject.GetComponent<Transform>();
        _animatorEnemy = animatorObject.GetComponent<Animator>();
        _enemies_A = FindObjectsOfType<HealthBarEnemy_A>();
    }

    public void Damage_A1()
    {
        foreach (var enemy in _enemies_A)
        {
            if (enemy != null)
            {
                _distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (_distance <= attackRange)
                {
                    if (enemy.HealthEnemy_A != 0)
                    {
                        enemy.TakeDamageByEnemy_A(_damageValueA);
                    }
                }
            }
        }
    }

    public void Damage_A2()
    {
        foreach (var enemy in _enemies_A)
        {
            if (enemy != null)
            {
                _distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (_distance <= attackRange2)
                {
                    if (enemy.HealthEnemy_A != 0)
                    {
                        enemy.TakeDamageByEnemy_A(_damageValueA2);
                    }
                }
            }
        }
    }
}
