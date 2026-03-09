using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int damage = 15;

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        damageable?.TakeDamage(damage);
    }
}
