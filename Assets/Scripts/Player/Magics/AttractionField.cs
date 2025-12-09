using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class AttractionField : MonoBehaviour
{    
    [Header("References")]
    [SerializeField] private AudioSource audioSource;

    private MagicData magicData;
    private Vector3 castDirection;
    private PlayerController player;

    private SphereCollider sphereCollider;
    private Material sphereMaterial;

    private float totalTime;
    private float startScale;
    private float endScale = 0.01f;

    private HashSet<EnemyController> affectedEnemies = new HashSet<EnemyController>();

    public void Initialize(MagicData data, Vector3 direction, PlayerController playerController)
    {
        magicData = data;
        castDirection = direction.normalized;
        player = playerController;
    }

    private void Start()
    {
        Radio.Instance.PlaySFX("SFX/MagicAttraction", audioSource);

        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) sphereMaterial = renderer.material;
        
        startScale = magicData.range * 2f;
        Debug.Log("AttractionField scale:" + startScale);
        transform.localScale = Vector3.one * startScale;
        
    }

    private void Update()
    {
        if (magicData == null) return;

        totalTime += Time.deltaTime;

        float attractionTime = totalTime / magicData.attractionPullAnimationTime;

        if (attractionTime >= 1f)
        {
            Destroy(gameObject);
            return;
        }

        float currentScale = Mathf.Lerp(startScale, endScale, attractionTime);
        transform.localScale = Vector3.one * currentScale;

        if (sphereMaterial != null)
        {
            Color color = sphereMaterial.color;
            color.a = Mathf.Lerp(1f, 0f, attractionTime);
            sphereMaterial.color = color;
        }

        foreach (EnemyController enemy in affectedEnemies)
        {
            if (enemy == null) continue;

            PullEnemy(enemy, attractionTime);
        }
    }

    private void PullEnemy(EnemyController enemy, float attractionTime)
    {
        Vector3 playerPos = player.transform.position;

        Vector3 enemyPosition = enemy.agent.nextPosition;

        float finalDistance = magicData.attractionPullMinDistance;

        Vector3 direction = (enemyPosition - playerPos).normalized;

        Vector3 finalPosition = playerPos + direction * finalDistance;

        Vector3 newPosition = Vector3.Lerp(enemyPosition, finalPosition, attractionTime);

        enemy.agent.nextPosition = newPosition;
        enemy.transform.position = enemy.agent.nextPosition;

        if (attractionTime >= 0.99f)
        {
            StatusEffectHandler handler = enemy.GetComponent<StatusEffectHandler>();
            if (handler != null)
            {
                handler.ApplyStatus(new StunnedEffect(magicData.attractionStunDuration));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyController enemyController = other.GetComponent<EnemyController>();
        StatusEffectHandler statusEffectHandler = other.GetComponent<StatusEffectHandler>();
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();

        if (enemyController == null || statusEffectHandler == null) return;
        if (affectedEnemies.Contains(enemyController)) return;

        Vector3 dirToEnemy = (enemyController.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(castDirection, dirToEnemy);

        if (angle <= magicData.attractionConeAngle)
        {
            affectedEnemies.Add(enemyController);

            if(healthSystem != null)
            {
                float attractionTime = magicData.attractionPullAnimationTime;

                healthSystem.ActivateDoubleDamage(attractionTime);
            }

            enemyController.isFreezed = true;

            if (enemyController.agent != null)
            {
                enemyController.agent.updatePosition = false;
                enemyController.agent.updateRotation = false;
                enemyController.agent.isStopped = true;
            }
            else
            {
                Debug.Log("The NavMeshAgent reference was not found!");
            }
        }
    }
}

