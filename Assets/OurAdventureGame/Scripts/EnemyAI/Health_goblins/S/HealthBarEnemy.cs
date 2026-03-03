using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarEnemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    private int healthEnemy = 80;

    [Header("HealthBar")]
    [SerializeField] private HealthBar_S healthBar_S;
    [SerializeField] private GameObject HB_active;

    public int HealthEnemy
    {
        get
        {
            return healthEnemy;
        }
        set
        {
            healthEnemy = value;
            if (healthEnemy <= 0)
            {
                healthEnemy = 0;
                HB_active.SetActive(false);
                DieEnemy();
            }
            healthBar_S.UpdateHealthBar(maxHealth, healthEnemy);
        }
    }

    private void Start()
    {
        HealthEnemy = maxHealth;
        Debug.Log(HealthEnemy + " -- здоровья");
    }

    public void TakeDamageByEnemy(int damageValue)
    {
        HealthEnemy -= damageValue;
        Debug.Log(HealthEnemy + " -- здоровья");

    }

    public void DieEnemy()
    {

        Debug.Log("Враг умер");
        _animator.SetTrigger("death");
        GetComponent<Collider>().enabled = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<enemyAI>().enabled = false;
        GetComponent<Hammer_attack>().enabled = false;

        DOTween.Kill(transform);

        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
