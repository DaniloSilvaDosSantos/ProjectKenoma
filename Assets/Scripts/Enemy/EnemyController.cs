using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyController : MonoBehaviour, IEntityController
{
    [Header("Enemy Data")]
    public EnemyData enemyData;

    [Header("References")]
    public NavMeshAgent agent;
    public HealthSystem health;
    public EnemyStateMachine stateMachine;
    public Transform playerTarget;

    [Header("Runtime State")]
    public bool isAlive = true;
    public bool isFreezed = false;

    private Coroutine levitationCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<HealthSystem>();
        stateMachine = GetComponent<EnemyStateMachine>();
    }

    private void Start()
    {
        if (enemyData != null)
        {
            agent.speed = enemyData.movementSpeed;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
        else
        {
            Debug.LogWarning("EnemyController: No player found in scene!");
        }

        if (stateMachine != null)
        {
            stateMachine.Initialize(this);
        }
    }

    public EntityData GetEntityData() => enemyData;

    public void OnEntityDeath()
    {
        if (!isAlive) return;
        isAlive = false;

        Debug.Log($"{gameObject.name} died!");
        agent.isStopped = true;

        if (stateMachine != null)
        {
            stateMachine.ChangeState(new EnemyDeadState(this, stateMachine));
        }
        else
        {
            Destroy(gameObject, 1f);
        }
    }

        public void ApplyLevitationEffect(MagicData magicData)
    {
        if (levitationCoroutine != null)
            StopCoroutine(levitationCoroutine);

        levitationCoroutine = StartCoroutine(LevitationEffectRoutine(magicData));
    }

    private IEnumerator LevitationEffectRoutine(MagicData magicData)
    {
        if (!isAlive) yield break;

        Debug.Log(gameObject.name + " is levitating!");

        isFreezed = true;
        agent.isStopped = true;

        agent.updatePosition = false;
        agent.updateRotation = false;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * magicData.liftHeight;

        float duration = magicData.effectDuration;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos,
                Mathf.PingPong(elapsed, duration / 2f) / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        float fallTime = 0.5f;
        Vector3 fallStart = transform.position;
        Vector3 fallEnd = new Vector3(fallStart.x, startPos.y, fallStart.z);
        elapsed = 0f;

        while (elapsed < fallTime)
        {
            transform.position = Vector3.Lerp(fallStart, fallEnd, elapsed / fallTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent.nextPosition = hit.position;
        }

        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;
        isFreezed = false;
        levitationCoroutine = null;

        Debug.Log(gameObject.name + " recovered from levitation.");
    }
}

