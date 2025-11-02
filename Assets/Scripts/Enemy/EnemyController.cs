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

        float startOffset = agent.baseOffset;
        float targetOffset = startOffset + magicData.liftHeight;

        float duration = magicData.effectDuration;
        float elapsed = 0f;

        float riseElapsed = 0f;
        while (riseElapsed < magicData.riseTime)
        {
            agent.baseOffset = Mathf.Lerp(startOffset, targetOffset, riseElapsed / magicData.riseTime);
            riseElapsed += Time.deltaTime;
            yield return null;
        }
        agent.baseOffset = targetOffset;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        float vel = 0f;
        float gravity = enemyData.gravity;
        float current = agent.baseOffset;

        while (current > startOffset)
        {
            vel += gravity * Time.deltaTime;
            current += vel * Time.deltaTime;
            agent.baseOffset = current;
            yield return null;
        }

        agent.baseOffset = startOffset;
        agent.isStopped = false;
        isFreezed = false;
        levitationCoroutine = null;

        Debug.Log(gameObject.name + " recovered from levitation.");
    }
}

