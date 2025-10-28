using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement Settings")]
    public float movementSpeed = 14f;
    public float gravity = -9.81f;
    [Space]

    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public float minLookAngle = -90f;
    public float maxLookAngle = 90f;
    [Space]

    [Header("Shotgun Settings")]
    public float shotgunAngle = 45f;
    [Space]
    public float shotgunMaxRangeD = 12f;
    public float shotgunRangeA = 2f;
    public float shotgunRangeB = 5f;
    public float shotgunRangeC = 8f;
    [Space]
    public float shotgunMaxDamage = 10f;
    public float shotgunCloseDamage = 7f;
    public float shotgunFarDamage = 5f;
    public float shotgunVeryFarDamage = 2f;
    [Space]

    [Header("Player Stats")]
    public float maxHealth = 100f;
    public float startingHealth = 100f;
}
