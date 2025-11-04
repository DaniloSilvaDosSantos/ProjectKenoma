using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Waves Database")]
public class WavesDatabase : ScriptableObject
{
    public WaveData[] waves;
}
