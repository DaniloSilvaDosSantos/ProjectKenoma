using UnityEngine;
using UnityEngine.Events;

public class PlayerLevelSystem : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxLevel = 99;

    [Header("XP")]
    [SerializeField] private int totalXp = 0;
    [SerializeField] private int xpNeededForNextLevel;

    [Header("Events")]
    [SerializeField] public UnityEvent<int> OnLevelUp;
    [SerializeField] public UnityEvent<int> OnXpChanged;
    [SerializeField] public UnityEvent OnUpgradeMenuRequested;

    private void Start()
    {
        xpNeededForNextLevel = GetRequiredXpUntilLevel(currentLevel + 1);
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
        while (currentLevel < maxLevel && totalXp >= xpNeededForNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        OnLevelUp?.Invoke(currentLevel);

        xpNeededForNextLevel = GetRequiredXpUntilLevel(currentLevel + 1);

        OnUpgradeMenuRequested?.Invoke();
    }

    private int CalculateXpNeeded(int level)
    {
        return 10 * level * level + 15 * level;
    }


    public int GetTotalXP() => totalXp;

    public int GetCurrentLevel() => currentLevel;

    public int GetRequiredXpUntilLevel(int level)
    {
        int sum = 0;
        for (int i = 1; i < level; i++)
        {
            sum += CalculateXpNeeded(i);
        }
        return sum;
    }

    public int GetCurrentLevelXP()
    {
        int previousLevelsXP = GetRequiredXpUntilLevel(currentLevel);
        return Mathf.Max(0, totalXp - previousLevelsXP);
    }

    public int GetCurrentLevelXPNeeded()
    {
        return CalculateXpNeeded(currentLevel);
    }
}
