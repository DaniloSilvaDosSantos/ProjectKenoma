using UnityEngine;

[CreateAssetMenu(fileName = "MagicUpgradesData", menuName = "Game Data/MagicUpgrades")]
public class MagicUpgradesData : ScriptableObject
{
    [Header("Levitation Upgrade")]
    public int levitationLevel = 0;

    public float[] levitationDurationValues;
    public float[] sphereFinalScaleValues;
}

