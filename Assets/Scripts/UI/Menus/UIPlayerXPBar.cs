using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerXPBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerLevelSystem levelSystem;
    [SerializeField] private UIUpgradeMenu upgradeMenu;

    [Header("UI Elements")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Animation Settings")]
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float maxSpeedMultiplier = 6f;
    [SerializeField] private float menuOpenDelay = 0.05f; 

    [Header("Flash Animation Settings")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.15f;

    private Coroutine fillRoutine;
    private Coroutine flashRoutine;
    private bool waitingForLevelUpMenu = false;
    private Color originalFillColor;

    private void Start()
    {
        if (levelSystem == null) levelSystem = FindAnyObjectByType<PlayerLevelSystem>();
        if (upgradeMenu == null) upgradeMenu = FindAnyObjectByType<UIUpgradeMenu>();

        originalFillColor = fillImage.color;

        UpdateXP(levelSystem.GetTotalXP());
        UpdateLevel(levelSystem.GetCurrentLevel());

        levelSystem.OnXpChanged.AddListener(UpdateXP);
        levelSystem.OnLevelUp.AddListener(UpdateLevel);
        levelSystem.OnUpgradeMenuRequested.AddListener(OnLevelUpTriggered);
    }

    private void OnLevelUpTriggered()
    {
        waitingForLevelUpMenu = true;
    }

    private void UpdateXP(int totalXp)
    {
        int currentXP = levelSystem.GetCurrentLevelXP();
        int xpNeeded = levelSystem.GetCurrentLevelXPNeeded();

        float targetFill = xpNeeded > 0 ? (float)currentXP / xpNeeded : 0f;

        xpText.text = $"{currentXP} / {xpNeeded} Spiritual Energy";

        if (fillRoutine != null) StopCoroutine(fillRoutine);
        fillRoutine = StartCoroutine(AnimateFill(targetFill));

        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashFill());
    }

    private IEnumerator AnimateFill(float target)
    {
        float start = fillImage.fillAmount;
        float distance = Mathf.Abs(target - start);

        float speed = baseSpeed + (distance * maxSpeedMultiplier);
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * speed;
            fillImage.fillAmount = Mathf.Lerp(start, target, time);
            yield return null;
        }

        fillImage.fillAmount = target;

        if (waitingForLevelUpMenu)
        {
            waitingForLevelUpMenu = false;

            yield return new WaitForSeconds(menuOpenDelay);

            upgradeMenu.OpenMenu(false);
        }
    }

    private IEnumerator FlashFill()
    {
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            float t = elapsed / flashDuration;
            float alpha = Mathf.Sin(t * Mathf.PI);

            fillImage.color = Color.Lerp(originalFillColor, flashColor, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        fillImage.color = originalFillColor;
    }

    private void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
        UpdateXP(levelSystem.GetTotalXP());
    }
}

