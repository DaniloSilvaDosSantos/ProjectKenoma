using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquestPointFlash : MonoBehaviour
{
    [Header("Flash Settings")]
    public List<Renderer> renderersToFlash = new List<Renderer>();
    public Color flashColor = Color.cyan;
    public float flashDuration = 2f;
    public float flashIntensity = 1f;

    private class OriginalMaterialData
    {
        public Color originalColor;
    }

    private Dictionary<Material, OriginalMaterialData> materialData = new Dictionary<Material, OriginalMaterialData>();

    private Coroutine flashRoutine;

    private void Awake()
    {
        VerifyRenderers(renderersToFlash);
    }

    private void VerifyRenderers(List<Renderer> renderers)
    {
        foreach (Renderer renderer in renderers)
        {
            if (renderer == null) continue;
            foreach (Material material in renderer.materials)
            {
                if(!IsStandardLitMaterial(material)) return;

                if (!materialData.ContainsKey(material))
                {
                    materialData[material] = new OriginalMaterialData
                    {
                        originalColor = material.color
                    };
                }
            }
        }
    }

    public void StartFlash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashLoop());
    }

    public void StopFlash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);

        foreach (var pair in materialData)
        {
            pair.Key.color = pair.Value.originalColor;
        }

        flashRoutine = null;
    }

    private IEnumerator FlashLoop()
    {
        while (true)
        {
            float elapsed = 0f;

            while (elapsed < flashDuration)
            {
                float time = Mathf.Sin(elapsed / flashDuration * Mathf.PI);

                foreach (var pair in materialData)
                {
                    Material mat = pair.Key;
                    Color baseColor = pair.Value.originalColor;

                    Color flash = Color.Lerp(baseColor, baseColor + (flashColor * flashIntensity),time);

                    mat.color = flash;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }
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