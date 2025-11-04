using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool IsValidSpawn(Transform player, float minDist, float maxDist)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance >= minDist && distance <= maxDist;
    }

    public void SpawnEnemy(GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}

