using UnityEngine;

public enum SpecialUpgradeType
{
    RecoverHealth
}

[CreateAssetMenu(fileName = "SpecialUpgradeData", menuName = "Game Data/SpecialUpgradeData")]
public class SpecialUpgradeData : ScriptableObject
{
    [Header("Settings")]
    [Range(0f, 1f)]
    public float recoverHealthPercentage = 0.3f;
}