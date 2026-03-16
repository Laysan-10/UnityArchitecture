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
        // 1. Игнорируем триггеры (чужие радиусы атак, зоны видимости)
        if (other.isTrigger) return;

        // 2. Игнорируем своих (по тегам)
        if (_isPlayerProjectile && other.CompareTag("Player")) return;
        if (!_isPlayerProjectile && other.CompareTag("Enemy")) return;

        // 3. Игнорируем пол/землю (опционально, если хотите, чтобы шар летел над землей и не взрывался об кочки)
        // Если у вашей земли есть тег "Ground" или "Terrain", раскомментируйте строчку ниже:
        // if (other.CompareTag("Terrain")) return;

        // Если шар долетел сюда, значит он во что-то врезался!
        Debug.Log($"<color=cyan>Снаряд столкнулся с: {other.name} (Его тег: {other.tag})</color>");

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(0, _damage); 
            Debug.Log($"<color=orange>УСПЕХ! Снаряд нанес {_damage} магического урона объекту {other.name}!</color>");
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}