using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    [Header("Limit Settings")]
    public int maxAliveEnemies = 80;
    private Queue<EnemyData> pendingSpawns = new Queue<EnemyData>();
    [Space]
    [Space]

    [Header("Wave Timer")]
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private bool waveTimerRunning = false;
    [SerializeField] private float waveTimer = 0f;

    [SerializeField] private int currentWaveIndex;
    public bool allowWaves = false;
    [SerializeField] private int currentRound;
    [SerializeField] private int totalAliveEnemies;

    [Header("End of Waves Settings")]
    [SerializeField] public GameObject endPrefab;
    [SerializeField] public float prefabSpawnDistance = 20f;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject.transform;

        if(wavesDB != null)
        {
            minSpawnDist = wavesDB.minSpawnDist;
            maxSpawnDist = wavesDB.maxSpawnDist;
            maxAliveEnemies = wavesDB.maxAliveEnemies;
        }

        StartCoroutine(CheckStart());
    }

    private void Update()
    {
        totalAliveEnemies = EnemyCounter.aliveEnemies;

        if (waveTimerRunning)
        {
            waveTimer -= Time.deltaTime;

            if (waveTimer < 0f) waveTimer = 0f;

            UpdateWaveTimerUI();
        }

        TrySpawnPending();
    }

    

    private void UpdateWaveTimerUI()
    {
        if (waveTimerText != null)
        {
            int minutes = Mathf.FloorToInt(waveTimer / 60f);
            int seconds = Mathf.FloorToInt(waveTimer % 60f);
            waveTimerText.text = $"{minutes:0}:{seconds:00}";
        }
    }

    private void TrySpawnPending()
    {
        if (pendingSpawns.Count == 0) return;

        if (EnemyCounter.aliveEnemies >= maxAliveEnemies) return;

        EnemyData enemyData = pendingSpawns.Dequeue();
        SpawnEnemy(enemyData);
    }

    IEnumerator CheckStart()
    {
        while (!allowWaves) yield return null;

        StartCoroutine(RunWaves());

        while (Time.timeScale < 1f)
        {
            yield return null;
        }

        Radio.Instance.PlayMusic("Music/Game");
    }

    private float CalculateTotalCooldown()
    {
        if (wavesDB == null || wavesDB.waves == null) return 0f;

        float total = 0f;

        foreach (var wave in wavesDB.waves)
        {
            if (wave == null) continue;

            total += wave.roundDelay * wave.numberOfRounds;
        }

        return total;
    }


    private IEnumerator RunWaves()
    {
        if (wavesDB == null || wavesDB.waves == null || wavesDB.waves.Length == 0)
        {
            Debug.Log("Waves database is empty.");
            yield break;
        }

        waveTimer = CalculateTotalCooldown();
        waveTimerRunning = true;


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

        waveTimerRunning = false;

        HandleEndOfWaves();
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
                if (EnemyCounter.aliveEnemies < maxAliveEnemies)
                {
                    SpawnEnemy(enemiesToSpawnInfo.enemy);
                }
                else
                {
                    pendingSpawns.Enqueue(enemiesToSpawnInfo.enemy);
                }
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

    private void HandleEndOfWaves()
    {
        Debug.Log("Starting The End Wave");

        Radio.Instance.StopMusic(true, 5f);

        FindAnyObjectByType<VFXVolumeController>()?.PlayEndingLevel(7f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemie in enemies)
        {
            HealthSystem healthSystem = enemie.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(9999f, false, 0, 0);
            }
        }

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            PlayerShotgun shotgun = playerObj.GetComponent<PlayerShotgun>();
            if (shotgun != null) shotgun.enabled = false;

            PlayerInteract interact = playerObj.GetComponent<PlayerInteract>();
            if (interact != null) interact.enabled = false;

            PlayerLevelSystem level = playerObj.GetComponent<PlayerLevelSystem>();
            if (level != null) level.enabled = false;
        }

        UIPlayerXPBar xpBar = FindAnyObjectByType<UIPlayerXPBar>();
        if (xpBar != null)
        {
            xpBar.enabled = false;
        }

        if (endPrefab != null && player != null)
        {
            Vector3 spawnPos = player.position + player.forward * prefabSpawnDistance;
            Instantiate(endPrefab, spawnPos, Quaternion.identity);
        }

        this.enabled = false;
    }
}
