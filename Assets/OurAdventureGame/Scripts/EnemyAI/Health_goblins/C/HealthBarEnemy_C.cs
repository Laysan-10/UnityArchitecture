using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarEnemy_C : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [Header("Health")]
    [SerializeField] private int maxHealth_C;
    private int healthEnemy_C = 70;

    [Header("HealthBar")]
    [SerializeField] private HealthBar_C healthBar_C;
    [SerializeField] private GameObject HB_active_C;

    public int HealthEnemy_C
    {
        get
        {
            return healthEnemy_C;
        }
        set
        {
            healthEnemy_C = value;
            if (healthEnemy_C <= 0)
            {
                healthEnemy_C = 0;
                HB_active_C.SetActive(false);
                DieEnemy_C();
            }
            healthBar_C.UpdateHealthBar_C(maxHealth_C, healthEnemy_C);
        }
    }

    private void Start()
    {
        HealthEnemy_C = maxHealth_C;
        Debug.Log(HealthEnemy_C + "");
    }

    public void TakeDamageByEnemy_C(int damageValue)
    {
        HealthEnemy_C -= damageValue;
        Debug.Log(HealthEnemy_C + "");

    }

    public void DieEnemy_C()
    {

        Debug.Log("���� ����");
        _animator.SetTrigger("death");
        GetComponent<Collider>().enabled = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<enemyAI>().enabled = false;
        GetComponent<Attack_goblin_C_Rang>().enabled = false;

        DOTween.Kill(transform);

        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
