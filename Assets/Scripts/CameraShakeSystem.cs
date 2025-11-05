using System.Collections;
using UnityEngine;

public class CameraShakeSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    
    [Header("Limits")]
    public float maxShakeStrenght = 0.25f;

    private Coroutine shakeRoutine;
    private Vector3 originalLocalPos;

    void Awake()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) Debug.Log("Main Camera don't have been found");

        originalLocalPos = mainCamera.transform.localPosition;
    }

    public void Shake(float duration, float amplitude)
    {
        amplitude = Mathf.Min(amplitude, maxShakeStrenght);

        if (shakeRoutine != null) StopCoroutine(shakeRoutine);
        shakeRoutine = StartCoroutine(DoShake(duration, amplitude));
    }

    private IEnumerator DoShake(float duration, float startAmplitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float time = elapsed / duration;

            float currentAmp = Mathf.Lerp(startAmplitude, 0f, time);

            Vector3 offset = Random.insideUnitSphere * currentAmp;
            transform.localPosition = originalLocalPos + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPos;
        shakeRoutine = null;
    }
}
