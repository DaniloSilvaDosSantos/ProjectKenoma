using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UltimateBlackHole : MonoBehaviour
{
    private MagicData data;
    private PlayerController controller;

    private float currentScale;
    private float currentAlpha = 1f;

    private Material sphereMaterial;
    private HashSet<HealthSystem> damagedEnemies = new HashSet<HealthSystem>();

    public void Initialize(MagicData magicData, PlayerController caster)
    {
        data = magicData;
        controller = caster;
    }

    private void Start()
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null) sphereMaterial = r.material;

        currentScale = data.prefabStartScale;
        transform.localScale = Vector3.one * currentScale;

        StartCoroutine(UltimateRoutine());
    }

    private IEnumerator UltimateRoutine()
    {
        while (currentScale < data.prefabFinalScale)
        {
            currentScale += data.prefabGrowSpeed * Time.deltaTime;
            transform.localScale = Vector3.one * currentScale;
            yield return null;
        }
        
        Explode();

        currentScale = data.prefabFinalScale;

        float t = 0f;

        while (t < data.ultimateDuration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;
        while (currentScale > data.prefabStartScale || currentAlpha > 0f)
        {
            currentScale -= data.prefabGrowSpeed * Time.deltaTime;
            currentScale = Mathf.Max(currentScale, data.prefabStartScale);
            transform.localScale = Vector3.one * currentScale;

            if (sphereMaterial != null)
            {
                currentAlpha -= data.prefabFadeSpeed * Time.deltaTime;
                currentAlpha = Mathf.Clamp01(currentAlpha);

                Color c = sphereMaterial.color;
                c.a = currentAlpha;
                sphereMaterial.color = c;
            }

            yield return null;
        }

        Debug.Log("Ples");

        EndUltimate();
        Destroy(gameObject);
    }

    private void Explode()
    {
        float radius = data.range;
        float damage = data.ultimateDamage;

        Vector3 center = controller.transform.position;

        Collider[] hits = Physics.OverlapSphere(center, radius);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            HealthSystem enemy = hit.GetComponent<HealthSystem>();
            if (enemy != null && !damagedEnemies.Contains(enemy))
            {
                damagedEnemies.Add(enemy);

                enemy.TakeDamage(damage, registerKillForMagic: false, 0f, 0f);
            }
        }
    }

    private void EndUltimate()
    {
        controller.health.SetInvulnerable(false);
        controller.playerMovement.isMovementLocked = false;
    }
}
