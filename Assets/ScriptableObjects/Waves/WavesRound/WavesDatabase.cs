using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Waves Database")]
public class WavesDatabase : ScriptableObject
{
    [Header("Spawn Variables")]
    public float minSpawnDist = 30f;
    public float maxSpawnDist = 120f;
    public int maxAliveEnemies = 80;
    [Space]

    [Header("All Waves")]
    public WaveData[] waves;
}
