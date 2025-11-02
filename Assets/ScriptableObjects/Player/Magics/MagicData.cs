using UnityEngine;

public enum MagicType { MagicLevitation, Other }

[CreateAssetMenu(fileName = "NewMagicData", menuName = "Game Data/MagicData")]
public class MagicData : ScriptableObject
{
    [Header("General Info")]
    public string magicName = "Unnamed";
    public MagicType type = MagicType.Other;
    public float cooldown = 8f;
    public float range = 20f;
    public GameObject prefab;

    [Header("Levitation Settings")]
    public float effectDuration = 9999f;
    public float levitationDuration = 4f;
    public float riseTime = 1f; 
    public float liftHeight = 4f;
    public float sphereStartScale = 0.01f;
    public float sphereFinalScale = 5f;
    public float sphereGrowSpeed = 4f;
    public float sphereFadeSpeed = 1.5f;
}

