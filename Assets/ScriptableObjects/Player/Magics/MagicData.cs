using UnityEngine;

public enum MagicCooldownType
{
    Time,
    KillCount
}

public enum MagicType
{
    MagicLevitation,
    MagicAttraction,
    MagicUltimate,
    Other
}

[CreateAssetMenu(fileName = "NewMagicData", menuName = "Game Data/MagicData")]
public class MagicData : ScriptableObject
{
    [Header("General Info")]
    public string magicName = "Unnamed";
    public MagicType type = MagicType.Other;

    [Header("Magic Visual Settings")]
    public Color magicColor = Color.magenta;

    [Header("Cooldown Settings")]
    public MagicCooldownType cooldownType = MagicCooldownType.Time;
    public float cooldown = 8f;
    public int killsRequired = 50;

    [Header("General Settings")]
    public float range = 20f;
    public GameObject prefab;    
    public float prefabStartScale = 0.01f;
    public float prefabFinalScale = 5f;
    public float prefabGrowSpeed = 4f;
    public float prefabFadeSpeed = 1.5f;

    [Header("Levitation Settings")]
    public float effectDuration = 9999f;
    public float levitationDuration = 4f;
    public float riseTime = 1f; 
    public float liftHeight = 4f;


    [Header("Attraction Settings")]
    public float attractionConeAngle = 40f;
    public float attractionPullMinDistance = 4f;
    public float attractionStunDuration = 1f;
    public float attractionPullAnimationTime = 1f;

    [Header("Ultimate Settings")]
    public float ultimateDamage = 9999f;
    public float ultimateDuration = 3f;
    public float ultimateDistanteFromPlayer = 20f;
}

