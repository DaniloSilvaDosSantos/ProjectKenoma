using UnityEngine;

[CreateAssetMenu(fileName = "MagicUpgradesData", menuName = "Game Data/MagicUpgrades")]
public class MagicUpgradesData : ScriptableObject
{
    [Header("Levitation Upgrade")]
    public int levitationLevel = 0;

    public float[] levitationDurationValues;
    public float[] prefabFinalScaleValues;

    [Header("Attraction Upgrade")]
    public int attractionLevel = 0;

    public float[] attractionConeAngleValues;
    public float[] attractionRangeValues;
    public float[] attractionStunDurationValues;
}

