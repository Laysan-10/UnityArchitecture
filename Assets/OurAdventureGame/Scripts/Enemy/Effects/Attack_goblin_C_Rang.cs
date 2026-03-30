#if UNITY_EDITOR //чтобы был виден Gizmos
using UnityEditor;
using UnityEngine;
#endif //чтобы был виден Gizmos

public class Attack_goblin_C_Rang : MonoBehaviour
{
    public Player _player;
    [SerializeField] float attackRange = 7f;  // Расстояние, на котором атака считается успешной

    public void Damage()
    {
        if (_player != null)
        {
            float distance = Vector3.Distance(transform.position, _player.transform.position);

            // Проверяем, находится ли игрок в пределах допустимого расстояния для атаки
            if (distance <= attackRange)
            {
                _player.TakeDamage(10);
            }
        }
    }

#if UNITY_EDITOR // чтобы был виден Gizmos
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.up, attackRange);
    }
#endif // чтобы был виден Gizmos
}
