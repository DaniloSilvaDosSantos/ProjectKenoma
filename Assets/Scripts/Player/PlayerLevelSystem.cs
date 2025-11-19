using UnityEngine;
using UnityEngine.Events;

public class PlayerLevelSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIUpgradeMenu upgradeMenu;

    [Header("Level Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxLevel = 99;

    [Header("XP")]
    [SerializeField] private int totalXp = 0;
    [SerializeField] private int xpNeededForNextLevel;

    [Header("Events")]
    [SerializeField] private UnityEvent<int> OnLevelUp;
    [SerializeField] private UnityEvent<int> OnXpChanged;
    [SerializeField] private UnityEvent OnUpgradeMenuRequested;

    private void Start()
    {
        upgradeMenu = FindAnyObjectByType<UIUpgradeMenu>().GetComponent<UIUpgradeMenu>();

        xpNeededForNextLevel = CalculateXpNeeded(currentLevel);
    }

    public void AddXP(int amount)
    {
        if (currentLevel >= maxLevel) return;

        totalXp += amount;
        OnXpChanged?.Invoke(totalXp);

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (totalXp >= xpNeededForNextLevel && currentLevel < maxLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        OnLevelUp?.Invoke(currentLevel);

        xpNeededForNextLevel = CalculateXpNeeded(currentLevel);

        OnUpgradeMenuRequested?.Invoke();

        upgradeMenu.OpenMenu(false);
    }

    private int CalculateXpNeeded(int level)
    {
        return 10 * level * level + 15 * level;
    }
}

