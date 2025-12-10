using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [Header("References")]
    public HealthSystem health; 
    public List<Renderer> renderersToFlash = new List<Renderer>();

    [Header("Flash Settings")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    private class OriginalMaterialData
    {
        public Color originalColor;
        public float originalAlpha;
    }

    private Dictionary<Material, OriginalMaterialData> materialData = new Dictionary<Material, OriginalMaterialData>();
    private Coroutine flashRoutine;

    private void Awake()
    {
        if (health == null) health = GetComponent<HealthSystem>();

        VerifyRenderers(renderersToFlash);
    }

    private void VerifyRenderers(List<Renderer> renderersToFlash)
    {
        foreach (Renderer renderer in renderersToFlash)
        {
            if (renderer == null) continue;

            VerifyMaterialsInRenderer(renderer);
        }
    }

    private void VerifyMaterialsInRenderer(Renderer renderer)
    {
        foreach (Material material in renderer.materials)
        {
            if (IsStandardLitMaterial(material))
            {
                if (!materialData.ContainsKey(material))
                {
                    materialData[material] = new OriginalMaterialData
                    {
                        originalColor = material.color,
                        originalAlpha = material.color.a
                    };
                }
            }
        }
    }

    private void OnEnable()
    {
        if (health != null) health.OnDamaged.AddListener(OnTakeDamage);
    }

    private void OnDisable()
    {
        if (health != null) health.OnDamaged.RemoveListener(OnTakeDamage);
    }

    private void OnTakeDamage(float dmg)
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        float elapsed = 0f;


        foreach (var pair in materialData)
        {
            Material material = pair.Key;
            Color color = flashColor;
            color.a = 0f;
            material.color = color;
        }

        while (elapsed < flashDuration)
        {
            float t = elapsed / flashDuration;

            float alpha = Mathf.Sin(t * Mathf.PI);

            foreach (var pair in materialData)
            {
                Material material = pair.Key;
                OriginalMaterialData original = pair.Value;

                Color color = Color.Lerp(original.originalColor, flashColor, alpha);
                color.a = Mathf.Lerp(0f, 1f, alpha);
                material.color = color;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var pair in materialData)
        {
            Material material = pair.Key;
            OriginalMaterialData original = pair.Value;

            Color color = original.originalColor;
            color.a = original.originalAlpha;
            material.color = color;
        }

        flashRoutine = null;
    }

    private bool IsStandardLitMaterial(Material mat)
    {
        if (mat == null || mat.shader == null) return false;

        string shaderName = mat.shader.name;

        if (shaderName == "Standard") return true;

        if (shaderName == "Universal Render Pipeline/Lit") return true;

        return false;
    }
}
