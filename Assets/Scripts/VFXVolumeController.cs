using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class VFXVolumeController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume volume;
    [Space]
    [SerializeField] private Bloom bloom;
    [SerializeField] private Vignette vignette;
    [SerializeField] private ChromaticAberration chromaticAberration;

    [Header("Default Values")]
    public float defaultBloomIntensity = 0f;
    public float defaultVignetteIntensity = 0.25f;
    public float defaultChromaticAberrationIntensity = 0.1f;

    [Header("Starting To See")]
    public float startingBloomInitialIntensity = 10f;
    public float startingVignetteInitialIntensity = 1f;

    void Awake()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromaticAberration);

        bloom.intensity.Override(defaultBloomIntensity);
        vignette.intensity.Override(defaultVignetteIntensity);
        chromaticAberration.intensity.Override(defaultChromaticAberrationIntensity);

        PlayStartingToSee(4f, 2f);
    }

    public void PlayStartingToSee(float bloomDuration, float vignetteDuration)
    {
        StopAllCoroutines();
        StartCoroutine(StartingToSeeRoutine(bloomDuration, vignetteDuration));
    }

    IEnumerator StartingToSeeRoutine(float bloomDuration, float vignetteDuration)
    {
        bloom.intensity.Override(startingBloomInitialIntensity);
        vignette.intensity.Override(startingVignetteInitialIntensity);

        float bloomStart = startingBloomInitialIntensity;
        float vignetteStart = startingVignetteInitialIntensity;

        float bloomTimer = 0f;
        float vignetteTimer = 0f;

        while (bloomTimer < bloomDuration || vignetteTimer < vignetteDuration)
        {
            if (bloomTimer < bloomDuration)
            {
                bloomTimer += Time.deltaTime;
                float t = bloomTimer / bloomDuration;
                float value = Mathf.Lerp(bloomStart, defaultBloomIntensity, t);
                bloom.intensity.Override(value);
            }

            if (vignetteTimer < vignetteDuration)
            {
                vignetteTimer += Time.deltaTime;
                float t = vignetteTimer / vignetteDuration;
                float value = Mathf.Lerp(vignetteStart, defaultVignetteIntensity, t);
                vignette.intensity.Override(value);
            }

            yield return null;
        }

        bloom.intensity.Override(defaultBloomIntensity);
        vignette.intensity.Override(defaultVignetteIntensity);
    }
}