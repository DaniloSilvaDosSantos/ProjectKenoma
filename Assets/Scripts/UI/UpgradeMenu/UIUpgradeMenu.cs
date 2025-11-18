using UnityEngine;
using System.Collections.Generic;

public class UIUpgradeMenu : MonoBehaviour
{
    [Header("References")]
    public PlayerUpgradeManager upgradeManager;
    public GameObject menuPanel;
    public Transform upgradeOptionsHolder;
    public GameObject upgradeOptionPrefab;

    [Header("Settings")]
    public int upgradesPerRoll = 3;

    [SerializeField] private bool isConquestMenu = false;

    private void Awake()
    {
        menuPanel.SetActive(false);
    }

    public void OpenMenu(bool conquestMenu = false)
    {
        isConquestMenu = conquestMenu;

        Time.timeScale = 0f;
        menuPanel.SetActive(true);

        GenerateOptions();
    }

    public void CloseMenu()
    {
        foreach (Transform child in upgradeOptionsHolder) Destroy(child.gameObject);

        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void GenerateOptions()
    {
        List<System.Enum> availableUpgrades = new List<System.Enum>();

        if (!isConquestMenu)
        {
            foreach (PlayerUpgradeType type in System.Enum.GetValues(typeof(PlayerUpgradeType)))
            {
                if (!upgradeManager.IsPlayerUpgradeMaxed(type)) availableUpgrades.Add(type);
            }
        }
        else
        {
            foreach (ConquestUpgradeType type in System.Enum.GetValues(typeof(ConquestUpgradeType)))
            {
                if (!upgradeManager.IsConquestUpgradeMaxed(type)) availableUpgrades.Add(type);
            }
        }

        // Choicing The Upgrade Options
        int maxUpgradeOptions = Mathf.Min(upgradesPerRoll, availableUpgrades.Count);

        List<System.Enum> chosenUpgrades = new List<System.Enum>();
        while (chosenUpgrades.Count < maxUpgradeOptions)
        {
            var upgradePick = availableUpgrades[Random.Range(0, availableUpgrades.Count)];
            if (!chosenUpgrades.Contains(upgradePick)) chosenUpgrades.Add(upgradePick);
        }

        // Creating The UI
        foreach (var upgrade in chosenUpgrades)
        {
            GameObject upgradeObject = Instantiate(upgradeOptionPrefab, upgradeOptionsHolder);
            UIUpgradeOption ui = upgradeObject.GetComponent<UIUpgradeOption>();

            string title = upgrade.ToString();

            ui.Setup(
                title,
                () => 
                {
                    ApplyUpgrade(upgrade);
                    CloseMenu();
                }
            );
        }
    }

    void ApplyUpgrade(System.Enum upgrade)
    {
        if (!isConquestMenu)
        {
            upgradeManager.ApplyPlayerUpgrade((PlayerUpgradeType)upgrade);
        }
        else
        {
            upgradeManager.ApplyConquestUpgrade((ConquestUpgradeType)upgrade);
        }
    }
}
