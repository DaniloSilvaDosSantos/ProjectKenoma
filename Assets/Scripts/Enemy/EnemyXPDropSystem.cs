using UnityEngine;

public class EnemyXPDropSystem : MonoBehaviour
{
    [Header("XP Drop Settings")]
    public GameObject xpDropPrefab;

    public void DropXP(EnemyData data, Transform enemy, Transform player)
    {
        int drops = Mathf.Max(1, data.xpDropCount);
        int xpPerDrop = Mathf.CeilToInt((float)data.xpValue / drops);

        Vector3 toPlayer = (player.position - enemy.position).normalized;
        Vector3 baseDir = -toPlayer;

        for (int i = 0; i < drops; i++)
        {
            GameObject orb = Instantiate(xpDropPrefab, enemy.position, Quaternion.identity);

            XPDropBehaviour behaviour = orb.GetComponent<XPDropBehaviour>();

            float angle = Random.Range(-45f, 45f);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * baseDir;

            Debug.Log("DropingXP");
            behaviour.Initialize(xpPerDrop, dir);
        }
    }
}

