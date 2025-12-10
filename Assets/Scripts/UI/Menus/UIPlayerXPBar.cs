using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum XPBarState
{
    Idle,
    Filling,
    LevelUpFillToFull,
    LevelUpWaitMenu,
    LevelUpReset,
}


public class UIPlayerXPBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerLevelSystem levelSystem;
    [SerializeField] private UIUpgradeMenu upgradeMenu;

    [Header("UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Animation")]
    [SerializeField] private float fillSpeed = 3f;
    [SerializeField] private float maxSpeedMultiplier = 6f;
    [SerializeField] private float menuOpenDelay = 0.05f;

    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.25f;

    private float visualFill = 0f;
    private float visualTargetFill = 0f;

    private Color originalFillColor;
    private Coroutine flashRoutine;

    private XPBarState state = XPBarState.Idle;
    private Queue<int> levelUpQueue = new Queue<int>();

    private void Start()
    {
        if (levelSystem == null) levelSystem = FindAnyObjectByType<PlayerLevelSystem>();
        if (upgradeMenu == null) upgradeMenu = FindAnyObjectByType<UIUpgradeMenu>();

        originalFillColor = fillImage.color;

        visualFill = 0f;

        SetupEvents();
        RefreshFromRealXP();
    }

    private void SetupEvents()
    {
        levelSystem.OnXpChanged.AddListener(OnXPChanged);
        levelSystem.OnLevelUp.AddListener(OnLevelChanged);
        levelSystem.OnLevelUpVisual.AddListener(OnLevelUpRequested);
    }

    private void Update()
    {
        switch (state)
        {
            case XPBarState.Idle:
            case XPBarState.Filling:
                AnimateToTarget();
                break;

            case XPBarState.LevelUpFillToFull:
                FillToFull();
                break;

            case XPBarState.LevelUpReset:
                AnimateToTarget();
                break;
        }
    }

    private void OnXPChanged(int totalXP)
    {
        RefreshFromRealXP();

        if (state == XPBarState.Idle || state == XPBarState.Filling)
        {
            state = XPBarState.Filling;
        }

        TriggerFlash();
    }

    private void OnLevelChanged(int newLevel)
    {
        levelText.text = newLevel.ToString();
        RefreshFromRealXP();
    }

    private void OnLevelUpRequested()
    {
        levelUpQueue.Enqueue(1);

        if (state != XPBarState.LevelUpFillToFull && state != XPBarState.LevelUpWaitMenu && state != XPBarState.LevelUpReset)
        {
            StartCoroutine(ProcessLevelUps());
        }
    }

    private IEnumerator ProcessLevelUps()
    {
        while (levelUpQueue.Count > 0)
        {
            state = XPBarState.LevelUpFillToFull;

            yield return new WaitUntil(() => visualFill >= 0.999f);

            state = XPBarState.LevelUpWaitMenu;

            if (menuOpenDelay > 0) yield return new WaitForSecondsRealtime(menuOpenDelay);

            upgradeMenu.OpenMenu(false);

            yield return new WaitUntil(() => !upgradeMenu.upgradeMenuPanel.activeSelf);

            levelSystem.pendingUpgradeMenus = Mathf.Max(0, levelSystem.pendingUpgradeMenus - 1);

            RefreshFromRealXP();
            state = XPBarState.LevelUpReset;

            yield return new WaitUntil(() => Mathf.Abs(visualFill - visualTargetFill) < 0.001f);

            levelUpQueue.Dequeue();
        }

        state = XPBarState.Idle;
    }

    private void RefreshFromRealXP()
    {
        int curXP = levelSystem.GetCurrentLevelXP();
        int needXP = levelSystem.GetCurrentLevelXPNeeded();

        visualTargetFill = needXP > 0 ?
            (float)curXP / needXP : 0f;

        xpText.text = $"{curXP} / {needXP} Spiritual Energy";
    }

    private void AnimateToTarget()
    {
        float diff = Mathf.Abs(visualTargetFill - visualFill);
        float speed = fillSpeed + (diff * maxSpeedMultiplier);
        float delta = speed * Time.unscaledDeltaTime;

        visualFill = Mathf.MoveTowards(visualFill, visualTargetFill, delta);
        fillImage.fillAmount = visualFill;
    }

    private void FillToFull()
    {
        float diff = Mathf.Abs(1f - visualFill);
        float speed = fillSpeed + (diff * maxSpeedMultiplier);
        float delta = speed * Time.unscaledDeltaTime;

        visualFill = Mathf.MoveTowards(visualFill, 1f, delta);
        fillImage.fillAmount = visualFill;
    }

    private void TriggerFlash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        float t = 0f;

        while (t < flashDuration)
        {
            float progress = t / flashDuration;
            float alpha = Mathf.Sin(progress * Mathf.PI);

            fillImage.color =
                Color.Lerp(originalFillColor, flashColor, alpha);

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        fillImage.color = originalFillColor;
        flashRoutine = null;
    }
}
