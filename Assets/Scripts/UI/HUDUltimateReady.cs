using UnityEngine;
using TMPro;

public class HUDUltimateReady : MonoBehaviour
{
    [Header("Flash Colors")]
    public Color normalColor = Color.red;
    public Color highlightColor = Color.white;

    [Header("Flash Speed")]
    public float flashSpeed = 4f;

    private TextMeshProUGUI text;
    private bool isFlashing = false;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Radio.Instance.PlaySFX("SFX/ConquestPointSpawn");
    }

    private void Update()
    {
        if (!isFlashing) return;

        float t = (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f;
        text.color = Color.Lerp(normalColor, highlightColor, t);
    }

    public void SetHUDState(bool active)
    {
        if (active)
        {
            isFlashing = true;
            gameObject.SetActive(true);
        }
        else
        {
            isFlashing = false;
            gameObject.SetActive(false);
        }
    }
}

