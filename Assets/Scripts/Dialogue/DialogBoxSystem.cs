using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxSystem : MonoBehaviour
{
    public CanvasGroup dialogBoxGroup;
    public Image dialogBoxArt;
    public TextMeshProUGUI dialogText;

    public float fadeDuration = 0.2f;
    public float scaleFactor = 1.125f;
    public float scaleDuration = 0.05f;
    public float timeToPassDialog = 5f;

    [SerializeField] private DialogData dialogData;

    private int currentIndex = -1;
    private Vector3 defaultScale;

    private System.Action onDialogEndCallback;
    private Coroutine passDialogCoroutine;

    private void Start()
    {
        defaultScale = dialogBoxArt.rectTransform.localScale;
        dialogBoxGroup.alpha = 0f;
        //dialogBoxGroup.gameObject.SetActive(false);
    }

    public void StartDialog(DialogData data, System.Action onDialogEnd = null)
    {
        //dialogBoxGroup.gameObject.SetActive(true);

        currentIndex = -1;
        dialogData = data;
        onDialogEndCallback = onDialogEnd;

        if (passDialogCoroutine != null) StopCoroutine(passDialogCoroutine);

        if (timeToPassDialog >= 1f) passDialogCoroutine = StartCoroutine(PassDialogAfterTime(timeToPassDialog)); 

        StartCoroutine(FadeIn());
        ShowNextDialog();
        StartCoroutine(ScaleBox());
    }

    public void OnAdvancePressed()
    {
        if (currentIndex + 1 < dialogData.entries.Count)
        {
            StartCoroutine(ChangeDialog());
        }
        else
        {
            StartCoroutine(ScaleBox());
            StartCoroutine(FadeOutAndClose());
        }
    }

    private void ShowNextDialog()
    {
        currentIndex++;
        if (currentIndex >= dialogData.entries.Count) return;

        var entry = dialogData.entries[currentIndex];
        dialogText.text = entry.text;
        dialogText.color = entry.textColor;
    }

    private IEnumerator ChangeDialog()
    {
        yield return StartCoroutine(FadeText(0f));
        ShowNextDialog();
        yield return StartCoroutine(ScaleBox());
        yield return StartCoroutine(FadeText(1f));
    }

    private IEnumerator FadeText(float targetAlpha)
    {
        float t = 0f;
        Color originalColor = dialogText.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            dialogText.color = Color.Lerp(originalColor, targetColor, t / fadeDuration);
            yield return null;
        }

        dialogText.color = targetColor;
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        dialogBoxGroup.alpha = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            dialogBoxGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        dialogBoxGroup.alpha = 1f;
    }

    private IEnumerator FadeOutAndClose()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            dialogBoxGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        dialogBoxGroup.alpha = 0f;
        //dialogBoxGroup.gameObject.SetActive(false);

        onDialogEndCallback?.Invoke();
    }

    private IEnumerator ScaleBox()
    {
        float t = 0f;
        Vector3 targetScale = defaultScale * scaleFactor;

        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            dialogBoxArt.rectTransform.localScale = Vector3.Lerp(defaultScale, targetScale, t / scaleDuration);
            yield return null;
        }

        t = 0f;
        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            dialogBoxArt.rectTransform.localScale = Vector3.Lerp(targetScale, defaultScale, t / scaleDuration);
            yield return null;
        }

        dialogBoxArt.rectTransform.localScale = defaultScale;
    }

    private IEnumerator PassDialogAfterTime(float timer)
    {
        while (true)
        {
            if (currentIndex + 1 >= dialogData.entries.Count) yield break;

            yield return new WaitForSeconds(timer);

            OnAdvancePressed();

            if (currentIndex + 1 >= dialogData.entries.Count) yield break;
        }
    }
}
