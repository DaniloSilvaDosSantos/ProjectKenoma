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
}

