using System.Collections.Generic;
using UnityEngine;

public class MagicAuraColorController : MonoBehaviour
{
    [Header("Renderers")]
    public List<Renderer> auraRenderers = new List<Renderer>();

    private class OriginalMaterialData
    {
        public Color originalColor;
        public float originalAlpha;
        public string propertyName;
    }

    private Dictionary<Material, OriginalMaterialData> materialData = new Dictionary<Material, OriginalMaterialData>();

    private readonly string[] validColorProperties = { "_Color", "Color", "color" };

    private void Awake()
    {
        InitializeMaterials();
    }

    private void InitializeMaterials()
    {
        foreach (Renderer renderer in auraRenderers)
        {
            if (renderer == null) continue;

            foreach (Material material in renderer.materials)
            {
                string property = GetValidColorProperty(material);

                if (property != null)
                {
                    if (!materialData.ContainsKey(material))
                    {
                        Color baseColor = material.GetColor(property);

                        materialData[material] = new OriginalMaterialData
                        {
                            originalColor = baseColor,
                            originalAlpha = baseColor.a,
                            propertyName = property
                        };
                    }
                }
            }
        }
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
}
