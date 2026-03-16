using UnityEngine;

public class DummyDebug : MonoBehaviour
{
    private void Start()
    {
        var health = GetComponent<HealthComponent>();
        health.Core.OnHealthChanged += (current, max) => {
            Debug.Log($"<color=green>Куб получил урон! Текущее здоровье: {current} / {max}</color>");
        };
        health.Core.OnDeath += () => {
            Debug.Log("<color=red>Куб УНИЧТОЖЕН!</color>");
            Destroy(gameObject);
        };
    }
}