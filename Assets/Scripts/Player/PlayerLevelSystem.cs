using UnityEngine;
using UnityEngine.Events;

public class PlayerLevelSystem : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxLevel = 99;
    public int pendingUpgradeMenus = 0;

    [Header("XP")]
    [SerializeField] private int totalXp = 0;
    [SerializeField] private int xpNeededForNextLevel;

    [Header("Events")]
    public UnityEvent<int> OnLevelUp;
    public UnityEvent<int> OnXpChanged;
    public UnityEvent OnUpgradeMenuRequested;

    [SerializeField] public UnityEngine.Events.UnityEvent OnLevelUpVisual;

    [Header("Debug")]
    [SerializeField] private bool isDebug = false;
    [SerializeField] private KeyCode inputXP = KeyCode.X;

    private void Start()
    {
        xpNeededForNextLevel = GetRequiredXpUntilLevel(currentLevel + 1);
    }

    void Update()
    {
        //DEBUG
        if(isDebug && Input.GetKeyDown(inputXP)) AddXP(20);
    }

    public void AddXP(int amount)
    {
        if (currentLevel >= maxLevel) return;

        totalXp += amount;
        OnXpChanged?.Invoke(totalXp);

        Radio.Instance.PlaySFX("SFX/PickXP");

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

        pendingUpgradeMenus++;

        xpNeededForNextLevel = GetRequiredXpUntilLevel(currentLevel + 1);

        OnXpChanged?.Invoke(totalXp);

        OnUpgradeMenuRequested?.Invoke();

        OnLevelUpVisual?.Invoke();
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
