using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private GameObject impactEffect;

    private float _damage;

    public void Setup(float magicDamage)
    {
        _damage = magicDamage;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            return; 
        }
        
        Debug.Log($"Фаербол коснулся: {other.name}");


        // Если попали во что-то с интерфейсом урона
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(0, _damage); // Наносим магический урон
            Debug.Log($"Фаербол попал в {other.name}!");
        }

        // Спавн частиц (потом можно добавить)
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}