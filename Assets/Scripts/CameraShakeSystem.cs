using System.Collections;
using UnityEngine;

public class CameraShakeSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    [Header("Limits")]
    public float maxShakeStrenght = 1f;
    [Space]

    private Coroutine shakeRoutine;
    private Vector3 originalLocalPos;
    private float currentAmplitude;

    void Awake()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.Log("Main Camera don't have been found");
        } 

        originalLocalPos = mainCamera.transform.localPosition;
    }

    public void Shake(float duration, float amplitude)
    {
        amplitude = Mathf.Min(amplitude, maxShakeStrenght);

        if (amplitude <= currentAmplitude) return;

        currentAmplitude = amplitude;

        if (shakeRoutine != null) StopCoroutine(shakeRoutine);
        shakeRoutine = StartCoroutine(DoShake(duration));
    }

    private IEnumerator DoShake(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float time = elapsed / duration;

            float currentAmp = Mathf.Lerp(currentAmplitude, 0f, time);

            if (currentAmp < 0.00001f) currentAmp = 0f;

            currentAmplitude = currentAmp;

            Vector3 offset = Random.insideUnitSphere * currentAmp;
            transform.localPosition = originalLocalPos + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPos;
        currentAmplitude = 0f;
        shakeRoutine = null;
    }
}
