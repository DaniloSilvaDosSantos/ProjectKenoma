using UnityEngine;

public class EntityData : ScriptableObject
{
    [Header("Stats Settings")]
    public float maxHealth = 100f;
    public float startingHealth = 100f;
    [Space]

    [Header("Movement Settings")]
    public float movementSpeed = 14f;
    public float gravity = -9.81f;
    
}

