using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class LevitationField : MonoBehaviour
{
    private MagicData magicData;

    private float currentScale;
    private float fade = 1f;
    private Material sphereMaterial;
    private SphereCollider sphereCollider;
    private HashSet<EnemyController> affectedEnemies = new HashSet<EnemyController>();

    public void Initialize(MagicData data)
    {
        magicData = data;
    }

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) sphereMaterial = renderer.material;

        transform.localScale = Vector3.one * magicData.sphereStartScale;
        currentScale = magicData.sphereStartScale;
    }

    private void Update()
    {
        if (magicData == null) return;

        if (currentScale < magicData.sphereFinalScale)
        {
            currentScale += magicData.sphereGrowSpeed * Time.deltaTime;
            transform.localScale = Vector3.one * currentScale;
        }
        else
        {
            fade -= magicData.sphereFadeSpeed * Time.deltaTime;

            if (sphereMaterial != null)
            {
                Color color = sphereMaterial.color;
                color.a = Mathf.Clamp01(fade);
                sphereMaterial.color = color;
            }

            if (fade <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null && !affectedEnemies.Contains(enemy))
        {
            affectedEnemies.Add(enemy);
            StartCoroutine(ApplyLevitationEffect(enemy));
        }
    }

    private IEnumerator ApplyLevitationEffect(EnemyController enemy)
    {
        if (enemy == null || !enemy.isAlive) yield break;

        if (enemy.agent != null)
        {
            enemy.agent.isStopped = true;
            enemy.isFreezed = true;
        } 

        Vector3 startPos = enemy.transform.position;
        Vector3 targetPos = startPos + Vector3.up * magicData.liftHeight;

        float duration = magicData.effectDuration;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (enemy == null) yield break;
            enemy.transform.position = Vector3.Lerp(startPos, targetPos, Mathf.PingPong(elapsed, duration / 2f) / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (enemy != null && enemy.agent != null)
        {
            enemy.agent.isStopped = false;
            enemy.isFreezed = false;
        } 
    }
}
