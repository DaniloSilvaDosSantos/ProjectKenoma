using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Rounds Settings")]
    public int numberOfRounds = 4;
    public float roundDelay = 15f;

    [Header("Spawnable Enemies")]
    public List<EnemySpawnInfo> enemiesToSpawn;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public EnemyData enemy;
    public int count;
}
