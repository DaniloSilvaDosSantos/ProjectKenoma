using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawn Position Variance")]
    [SerializeField] float randomPositionVariance = 1f;

    void Start()
    {
        Renderer spawnerRenderer = GetComponent<Renderer>();
        spawnerRenderer.enabled = false;
    }

    public bool IsValidSpawn(Transform player, float minDist, float maxDist)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance >= minDist && distance <= maxDist;
    }

    public void SpawnEnemy(GameObject prefab)
    {
        Vector3 spawnPosition = new Vector3(
            transform.position.x + Random.Range(-randomPositionVariance, randomPositionVariance),
            transform.position.y,
            transform.position.z + Random.Range(-randomPositionVariance, randomPositionVariance)
            );

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}

