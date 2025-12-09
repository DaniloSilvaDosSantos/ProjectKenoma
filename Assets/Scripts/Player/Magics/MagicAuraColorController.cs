using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAuraColorController : MonoBehaviour
{
    [Header("Renderers")]
    public List<Renderer> auraRenderers = new List<Renderer>();

    [Header("Ready Effects")]
    public Color readyFlashColor = Color.white;
    public float readyScaleMultiplier = 1.33f;
    public float effectDuration = 2f;

    private class OriginalMaterialData
    {
        public Color originalColor;
        public float originalAlpha;
        public string propertyName;
    }

    private Coroutine readyRoutine;
    private Vector3 originalScale;

    private Dictionary<Material, OriginalMaterialData> materialData = new Dictionary<Material, OriginalMaterialData>();

    private readonly string[] validColorProperties = { "_Color", "Color", "color" };

    private void Awake()
    {
        originalScale = transform.localScale;
        InitializeMaterials();
    }

    private void InitializeMaterials()
    {
        foreach (Renderer renderer in auraRenderers)
        {
            if (renderer == null) 
                continue;

            foreach (Material material in renderer.materials)
            {
                TryRegisterMaterial(material);
            }
        }
    }

    private void TryRegisterMaterial(Material material)
    {
        if (material == null) return;

        string property = GetValidColorProperty(material);
        if (property == null) return;

        if (materialData.ContainsKey(material)) return;

        Color baseColor = material.GetColor(property);

        materialData[material] = new OriginalMaterialData
        {
            originalColor = baseColor,
            originalAlpha = baseColor.a,
            propertyName = property
        };
    }

    private string GetValidColorProperty(Material material)
    {
        if (material == null) return null;

        if (IsUnityStandardShader(material))
        {
            if (material.HasProperty("_Color")) return "_Color";

            return null;
        }

        foreach (string prop in validColorProperties)
        {
            if (material.HasProperty(prop)) return prop;
        }

        return null;
    }

    private bool IsUnityStandardShader(Material material)
    {
        string shader = material.shader.name;

        if (shader == "Standard") return true;
        if (shader == "Universal Render Pipeline/Lit") return true;

        return false;
    }

    public void SetAuraColor(Color newColor)
    {
        foreach (var pair in materialData)
        {
            Material material = pair.Key;
            OriginalMaterialData data = pair.Value;

            Color color = newColor;
            color.a = data.originalAlpha;

            material.SetColor(data.propertyName, color);
        }
    }

    public void ResetAuraColor()
    {
        foreach (var pair in materialData)
        {
            Material material = pair.Key;
            OriginalMaterialData data = pair.Value;

            Color color = data.originalColor;
            color.a = data.originalAlpha;

            material.SetColor(data.propertyName, color);
        }
    }

    public void SetAuraReadyState(bool isReady, Color magicColor)
    {
        if (!isReady)
        {
            StopReadyEffect();
            SetAuraColor(magicColor);
            return;
        }

        if (readyRoutine == null) readyRoutine = StartCoroutine(ReadyEffect(magicColor));
    }

    private void StopReadyEffect()
    {
        if (readyRoutine != null)
        {
            StopCoroutine(readyRoutine);
            readyRoutine = null;
        }

        ResetAuraColor();
    }

    private IEnumerator ReadyEffect(Color magicColor)
    {
        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;

            float normalized = elapsed / effectDuration % 1f;

            float time = (Mathf.Sin(normalized * Mathf.PI * 2f) + 1f) / 2f;

            Color lerped = Color.Lerp(magicColor, readyFlashColor, time);
            SetAuraColor(lerped);

            float scaleT = Mathf.Lerp(1f, readyScaleMultiplier, time);
            transform.localScale = originalScale * scaleT;

            yield return null;
        }
    }
}
