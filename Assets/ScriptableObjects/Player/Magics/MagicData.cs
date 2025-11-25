using UnityEngine;

public enum MagicType { MagicLevitation, MagicAttraction, Other }

[CreateAssetMenu(fileName = "NewMagicData", menuName = "Game Data/MagicData")]
public class MagicData : ScriptableObject
{
    [Header("General Info")]
    public string magicName = "Unnamed";
    public MagicType type = MagicType.Other;
    public float cooldown = 8f;
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
}

