using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private GameObject impactEffect;

    private float _damage;
    private bool _isPlayerProjectile; 

    public void Setup(float magicDamage, bool isPlayerProjectile)
    {
        _damage = magicDamage;
        _isPlayerProjectile = isPlayerProjectile;
        Debug.Log($"Снаряд создан. Урон: {_damage}. Стрелял игрок? {_isPlayerProjectile}");
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // МЕТОД СРАБОТАЕТ ТОЛЬКО ЕСЛИ НА ЭТОМ ОБЪЕКТЕ ЕСТЬ КОЛЛАЙДЕР С ГАЛОЧКОЙ "IS TRIGGER" И RIGIDBODY
    private void OnTriggerEnter(Collider other)
    {
        // 1. Проверяем, кто стрелял и кого игнорировать
        if (_isPlayerProjectile && other.CompareTag("Player")) return;
        if (!_isPlayerProjectile && other.CompareTag("Enemy")) return;

        Debug.Log($"<color=cyan>Снаряд столкнулся с: {other.name}</color>");

        // 2. Ищем интерфейс урона
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(0, _damage); 
            Debug.Log($"<color=orange>УСПЕХ! Снаряд нанес {_damage} магического урона объекту {other.name}!</color>");
        }
        else
        {
            Debug.Log($"<color=yellow>ПРЕДУПРЕЖДЕНИЕ: На объекте {other.name} нет интерфейса IDamageable!</color>");
        }

        // 3. Эффекты и уничтожение
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}