using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private GameObject impactEffect; // Эффект взрыва (если есть)

    private float _damage;

    public void Setup(float magicDamage)
    {
        _damage = magicDamage;
    }

    private void Start()
    {
        // Уничтожаем объект через N секунд, если он никуда не попал
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Движение вперед
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

        // Спавн частиц взрыва (по желанию)
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        // Уничтожаем снаряд при столкновении с чем угодно (земля, стена, враг)
        Destroy(gameObject);
    }
}