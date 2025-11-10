using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/ConquestPoints Database")]
public class ConquestPointsData : ScriptableObject
{
    [Header("Conquest Points Variables")]
    public float conquestPointTimeToActivate = 60f;
    public float conquestPointActiveDuration = 30f;
}
