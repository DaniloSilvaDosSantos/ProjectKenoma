using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerXPBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerLevelSystem levelSystem;

    [Header("UI Elements")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        if (levelSystem == null) levelSystem = FindAnyObjectByType<PlayerLevelSystem>();

        UpdateXP(levelSystem.GetTotalXP());
        UpdateLevel(levelSystem.GetCurrentLevel());

        levelSystem.OnXpChanged.AddListener(UpdateXP);
        levelSystem.OnLevelUp.AddListener(UpdateLevel);
    }

    private void UpdateXP(int totalXp)
    {
        int currentXP = levelSystem.GetCurrentLevelXP();
        int xpNeeded = levelSystem.GetCurrentLevelXPNeeded();

        float fill = xpNeeded > 0 ? (float)currentXP / xpNeeded : 0f;
        fillImage.fillAmount = fill;

        xpText.text = $"{currentXP} / {xpNeeded} Spiritual Energy";
    }

    private void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
        UpdateXP(levelSystem.GetTotalXP());
    }
}

