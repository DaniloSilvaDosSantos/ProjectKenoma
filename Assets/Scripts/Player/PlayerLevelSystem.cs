using UnityEngine;
using UnityEngine.Events;

public class PlayerLevelSystem : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentLevel = 1;
    public int maxLevel = 99;

    [Header("XP")]
    public int totalXp = 0;
    public int xpNeededForNextLevel;

    [Header("Events")]
    public UnityEvent<int> OnLevelUp;
    public UnityEvent<int> OnXpChanged;
    public UnityEvent OnUpgradeMenuRequested;

    private void Start()
    {
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

        //Time.timeScale = 0f;
        OnUpgradeMenuRequested?.Invoke();

        Debug.Log($"LEVEL UP! Now your level is {currentLevel}");
    }

    private int CalculateXpNeeded(int level)
    {
        return 10 * level * level + 15 * level;
    }
}

