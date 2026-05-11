using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private GameObject impactEffect;

    private float _physicalDamage;
    private float _magicDamage;
    private bool _isPlayerProjectile;
    private float _speedMultiplier = 1f;

    public void Setup(float magicDamage, bool isPlayerProjectile)
    {
        Setup(0f, magicDamage, isPlayerProjectile);
    }

    public void Setup(float physicalDamage, float magicDamage, bool isPlayerProjectile)
    {
        _physicalDamage = physicalDamage;
        _magicDamage = magicDamage;
        _isPlayerProjectile = isPlayerProjectile;
    }

    public void ApplyVisualOverride(Color tint, float speedMultiplier = 1f)
    {
        _speedMultiplier = Mathf.Max(0.1f, speedMultiplier);

        foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>(true))
        {
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = tint;
        }

        foreach (TrailRenderer trailRenderer in GetComponentsInChildren<TrailRenderer>(true))
        {
            trailRenderer.startColor = tint;
            trailRenderer.endColor = tint;
        }

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>(true))
        {
            if (renderer.material.HasProperty("_BaseColor"))
            {
                renderer.material.SetColor("_BaseColor", tint);
            }
            else if (renderer.material.HasProperty("_Color"))
            {
                renderer.material.SetColor("_Color", tint);
            }
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * _speedMultiplier * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (_isPlayerProjectile && other.CompareTag("Player"))
        {
            return;
        }

        if (!_isPlayerProjectile && other.CompareTag("Enemy"))
        {
            return;
        }

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_physicalDamage, _magicDamage);
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
