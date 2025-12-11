using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UltimateBlackHole : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Material sphereMaterial;

    private MagicData data;
    private PlayerController controller;

    private float currentScale;
    //private float currentAlpha = 1f;
    
    private HashSet<HealthSystem> damagedEnemies = new HashSet<HealthSystem>();

    public void Initialize(MagicData magicData, PlayerController caster)
    {
        data = magicData;
        controller = caster;
    }

    private void Start()
    {
        currentScale = data.prefabStartScale;
        transform.localScale = Vector3.one * currentScale;

        Radio.Instance.PlaySFX("SFX/MagicUltimate", audioSource);

        FindAnyObjectByType<VFXVolumeController>()?.PlayBlackHoleCast(2f);

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

        float time = 0f;

        while (time < data.ultimateDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Explode();

        //time = 0f;
        while (currentScale > data.prefabStartScale /* || currentAlpha > 0f*/)
        {
            currentScale -= data.prefabGrowSpeed * Time.deltaTime;
            currentScale = Mathf.Max(currentScale, data.prefabStartScale);
            transform.localScale = Vector3.one * currentScale;

            /*if (sphereMaterial != null)
            {
                currentAlpha -= data.prefabFadeSpeed * Time.deltaTime;
                currentAlpha = Mathf.Clamp01(currentAlpha);

                Color color = sphereMaterial.color;
                color.a = currentAlpha;
                sphereMaterial.color = color;
            }*/

            yield return null;
        }

        Explode();

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
