using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySaveData
{
    public string Id;          // Уникальный номер врага на сцене
    public Vector3 Position;   // Где стоял
    public float CurrentHp;    // Сколько здоровья осталось
    public bool IsDead;        // Жив или уже исчез
}

[Serializable]
public class SaveData
{
    public float PlayerHealth;
    public Vector3 PlayerPosition;

    // Список состояний всех врагов
    public List<EnemySaveData> EnemyStates = new List<EnemySaveData>();

    public SaveData()
    {
        PlayerHealth = 100f;
        PlayerPosition = Vector3.zero;
        EnemyStates = new List<EnemySaveData>();
    }
}