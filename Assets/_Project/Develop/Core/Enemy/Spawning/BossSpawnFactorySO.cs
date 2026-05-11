using UnityEngine;

[CreateAssetMenu(fileName = "BossFactory", menuName = "Factories/Boss Factory")]
public class BossSpawnFactorySO : EnemySpawnFactorySO
{
    public EnemyAgent SpawnBoss(
        Vector3 position,
        Quaternion rotation,
        Transform parent,
        string enemyId,
        BossElementController.BossElementType element)
    {
        EnemyAgent enemy = Spawn(position, rotation, parent, enemyId);
        if (enemy == null)
        {
            return null;
        }

        BossElementController elementController =
            enemy.GetComponent<BossElementController>() ??
            enemy.gameObject.AddComponent<BossElementController>();

        elementController.Initialize(element, BossElementController.BossWeaponMode.Melee);
        return enemy;
    }
}
