using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game Data/PlayerData")]
public class PlayerData : EntityData
{
    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public float minLookAngle = -90f;
    public float maxLookAngle = 90f;
    [Space]

    [Header("Shotgun Settings")]
    public float shotgunHalfAngle = 45f;
    public float shotgunCooldown = 2.5f;
    [Space]
    public float shotgunRangeA = 2f;
    public float shotgunRangeB = 5f;
    public float shotgunRangeC = 8f;
    public float shotgunMaxRangeD = 12f;
    [Space]
    
    public float shotgunMaxDamage = 10f;
    public float shotgunCloseDamage = 7f;
    public float shotgunFarDamage = 5f;
    public float shotgunVeryFarDamage = 2f;
}
