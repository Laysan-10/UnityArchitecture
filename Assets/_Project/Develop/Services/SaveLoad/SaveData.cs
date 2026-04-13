using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public Vector3 Position;
    public float CurrentHealth;
}

[Serializable]
public class EnemySaveData
{
    public string Id;
    public Vector3 Position;
    public float CurrentHp;
    public bool IsDead;
}

[Serializable]
public class SaveData
{
    public PlayerSaveData Player = new PlayerSaveData();
    public List<EnemySaveData> EnemyStates = new List<EnemySaveData>();

    public SaveData()
    {
        Player = new PlayerSaveData
        {
            Position = Vector3.zero,
            CurrentHealth = 100f
        };
        EnemyStates = new List<EnemySaveData>();
    }
}
