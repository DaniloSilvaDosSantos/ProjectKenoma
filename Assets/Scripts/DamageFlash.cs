using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [Header("References")]
    public HealthSystem health; 
    public List<Renderer> renderersToFlash = new List<Renderer>();

    [Header("Flash Settings")]
    public Color flashColor = Color.white;
    public float flashDuration = 0.2f;

    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();
    private Coroutine flashRoutine;

    private void Awake()
    {
        if (health == null) health = GetComponent<HealthSystem>();

        foreach (var renderer in renderersToFlash)
        {
            if (renderer != null) originalColors[renderer] = renderer.material.color;
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

    foreach (var renderer in renderersToFlash)
    {
        renderer.material.color = flashColor; 
    } 

    while (elapsed < flashDuration)
    {
        float time = elapsed / flashDuration;

        foreach (var renderer in renderersToFlash)
        {
            Color start = flashColor;
            Color end = originalColors[renderer];
            renderer.material.color = Color.Lerp(start, end, time);
        }

        elapsed += Time.deltaTime;
        yield return null;
    }

    foreach (var renderer in renderersToFlash)
    {
        renderer.material.color = originalColors[renderer];    
    } 

    flashRoutine = null;
}

}
