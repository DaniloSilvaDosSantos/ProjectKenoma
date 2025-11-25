using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgradesData", menuName = "Game Data/PlayerUpgrades")]
public class PlayerUpgradesData : ScriptableObject
{
    [Header("Max Health Upgrade")]
    public int maxHealthLevel = 0;
    public float[] maxHealthValues;  

    [Header("Movement Speed Upgrade")]
    public int movementSpeedLevel = 0;
    public float[] movementSpeedValues;

    [Header("Shotgun Damage Upgrade")]
    public int shotgunDamageLevel = 0;

    public float[] shotgunMaxDamageValues;
    public float[] shotgunCloseDamageValues;
    public float[] shotgunFarDamageValues;
    public float[] shotgunVeryFarDamageValues;
}

