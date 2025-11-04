using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WavesAndRoundSystem : MonoBehaviour
{
    [Header("Database")]
    public WavesDatabase wavesDB;

    [Header("Spawners")]
    public List<EnemySpawner> spawners;

    [Header("Spawn Distance Constraints")]
    public Transform player;
    public float minSpawnDist = 9f;
    public float maxSpawnDist = 36f;
    [Space]
    [Space]

    [SerializeField] private int currentWaveIndex;
    [SerializeField] private int currentRound;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject.transform;

        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        if (wavesDB == null || wavesDB.waves == null || wavesDB.waves.Length == 0)
        {
            Debug.Log("Waves database is empty.");
            yield break;
        }

        for (currentWaveIndex = 0; currentWaveIndex < wavesDB.waves.Length; currentWaveIndex++)
        {
            WaveData wave = wavesDB.waves[currentWaveIndex];
            Debug.Log($"Starting Wave {currentWaveIndex + 1} with {wave.numberOfRounds} rounds.");

            for (int round = 0; round < wave.numberOfRounds; round++)
            {
                currentRound = round;

                yield return new WaitForSeconds(wave.roundDelay);

                SpawnRound(wave, round);
            }

            Debug.Log($"Wave {currentWaveIndex + 1} finished.");
        }

        Debug.Log("! ALL WAVES FINISHED !");
    }

    private void SpawnRound(WaveData wave, int roundIndex)
    {
        Debug.Log($"Wave {currentWaveIndex + 1} - Round {roundIndex + 1} is spawning");

        foreach (var enemiesToSpawnInfo in wave.enemiesToSpawn)
        {
            if (enemiesToSpawnInfo == null || enemiesToSpawnInfo.enemy == null) continue;

            int total = enemiesToSpawnInfo.count;
            int rounds = Mathf.Max(1, wave.numberOfRounds);

            int basePerRound = total / rounds;
            int remainder = total % rounds;

            int amountThisRound = basePerRound + (roundIndex == rounds - 1 ? remainder : 0);

            if (amountThisRound <= 0) continue;

            for (int i = 0; i < amountThisRound; i++)
            {
                SpawnEnemy(enemiesToSpawnInfo.enemy);
            }
        }
    }

    private void SpawnEnemy(EnemyData enemyData)
    {
        if (enemyData == null || enemyData.enemyPrefab == null)
        {
            Debug.LogWarning("EnemyData or enemyPrefab is null.");
            return;
        }

        List<EnemySpawner> validSpawner = spawners.FindAll(s => s.IsValidSpawn(player, minSpawnDist, maxSpawnDist));
        if (validSpawner == null || validSpawner.Count == 0)
        {
            Debug.LogWarning("None valid spawner in the moment! spawn delayed.");
            return;
        }

        EnemySpawner chosen = validSpawner[Random.Range(0, validSpawner.Count)];
        chosen.SpawnEnemy(enemyData.enemyPrefab);
    }
}
