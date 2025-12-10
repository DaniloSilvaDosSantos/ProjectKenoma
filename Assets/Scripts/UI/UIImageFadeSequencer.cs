using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeSequencer : MonoBehaviour
{
    [Header("Image List")]
    public List<Image> images = new List<Image>();

    [Header("Fade Settings")]
    public float fadeInDuration = 1f;
    public float visibleDuration = 2f;
    public float fadeOutDuration = 1f;
    
    public float invisibleDuration = 0.5f;
    [Space]

    public string sceneToLoad = "MainMenu";

    void Start()
    {
        foreach (var image in images)
        {
            if (image != null)
            {
                Color color = image.color;
                color.a = 0f;
                image.color = color;
            }
        }

        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        foreach (var image in images)
        {
            if (image == null) continue;

            yield return StartCoroutine(FadeImage(image, 0f, 1f, fadeInDuration));

            yield return new WaitForSeconds(visibleDuration);

            yield return StartCoroutine(FadeImage(image, 1f, 0f, fadeOutDuration));

            yield return new WaitForSeconds(invisibleDuration);
        }

        GameController.Instance.ChangeScene(sceneToLoad, true);
    }

    IEnumerator FadeImage(Image image, float start, float end, float duration)
    {
        float time = 0f;
        Color color = image.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, time / duration);

            color.a = alpha;
            image.color = color;

            yield return null;
        }

        color.a = end;
        image.color = color;
    }
}