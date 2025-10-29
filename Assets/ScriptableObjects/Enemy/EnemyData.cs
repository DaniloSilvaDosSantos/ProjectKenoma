using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game Data/Enemy Data")]
public class EnemyData : EntityData
{
    [Header("Enemy Settings")]
    public float damage = 10f;
    public float attackCooldown = 2f;
    public float attackRange = 2f;
}

