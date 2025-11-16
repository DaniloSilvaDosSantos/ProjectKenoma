using UnityEngine;
using System.Collections.Generic;

public enum PlayerUpgradeType
{
    MaxHealth,
    MovementSpeed,
    ShotgunDamage
}

public enum ConquestUpgradeType
{
    Levitation
}

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("References")]
    public PlayerData playerStats;
    public PlayerUpgradesData playerUpgradesDB;
    public MagicData levitationMagicData;
    public MagicUpgradesData magicUpgradesDB;

    private void Start()
    {
        ApplyAllInitialValues();
    }

    // APPLYING THE INITIAL VALUES

    public void ApplyAllInitialValues()
    {
        // INITIAL PLAYER STATS
        
        playerStats.maxHealth = playerUpgradesDB.maxHealthValues[0];

        playerStats.movementSpeed = playerUpgradesDB.movementSpeedValues[0];

        playerStats.shotgunMaxDamage = playerUpgradesDB.shotgunMaxDamageValues[0];
        playerStats.shotgunCloseDamage = playerUpgradesDB.shotgunCloseDamageValues[0];
        playerStats.shotgunFarDamage = playerUpgradesDB.shotgunFarDamageValues[0];
        playerStats.shotgunVeryFarDamage = playerUpgradesDB.shotgunVeryFarDamageValues[0];

        // INITIAL MAGICS STATS

        levitationMagicData.levitationDuration = magicUpgradesDB.levitationDurationValues[0];
        levitationMagicData.sphereFinalScale = magicUpgradesDB.sphereFinalScaleValues[0];
    }

    // APPLY THE PLAYER LEVEL UP UPGRADES

    public void ApplyPlayerUpgrade(PlayerUpgradeType type)
    {
        switch (type)
        {
            case PlayerUpgradeType.MaxHealth:
                ApplyMaxHealthUpgrade();
                break;

            case PlayerUpgradeType.MovementSpeed:
                ApplyMovementSpeedUpgrade();
                break;

            case PlayerUpgradeType.ShotgunDamage:
                ApplyShotgunDamageUpgrade();
                break;
        }
    }

    private void ApplyMaxHealthUpgrade()
    {
        int level = playerUpgradesDB.maxHealthLevel;

        if (level + 1 < playerUpgradesDB.maxHealthValues.Length)
        {
            playerUpgradesDB.maxHealthLevel++;
            float newValue = playerUpgradesDB.maxHealthValues[playerUpgradesDB.maxHealthLevel];
            playerStats.maxHealth = newValue;
        }
    }

    private void ApplyMovementSpeedUpgrade()
    {
        int level = playerUpgradesDB.movementSpeedLevel;

        if (level + 1 < playerUpgradesDB.movementSpeedValues.Length)
        {
            playerUpgradesDB.movementSpeedLevel++;
            float newValue = playerUpgradesDB.movementSpeedValues[playerUpgradesDB.movementSpeedLevel];
            playerStats.movementSpeed = newValue;
        }
    }

    private void ApplyShotgunDamageUpgrade()
    {
        int level = playerUpgradesDB.shotgunDamageLevel;

        if (level + 1 < playerUpgradesDB.shotgunMaxDamageValues.Length)
        {
            playerUpgradesDB.shotgunDamageLevel++;

            int newLevel = playerUpgradesDB.shotgunDamageLevel;

            playerStats.shotgunMaxDamage = playerUpgradesDB.shotgunMaxDamageValues[newLevel];
            playerStats.shotgunCloseDamage = playerUpgradesDB.shotgunCloseDamageValues[newLevel];
            playerStats.shotgunFarDamage = playerUpgradesDB.shotgunFarDamageValues[newLevel];
            playerStats.shotgunVeryFarDamage = playerUpgradesDB.shotgunVeryFarDamageValues[newLevel];
        }
    }

    // APPLY CONQUEST UPGRADES

    public void ApplyConquestUpgrade(ConquestUpgradeType type)
    {
        switch (type)
        {
            case ConquestUpgradeType.Levitation:
                ApplyLevitationUpgrade();
                break;
        }
    }

    private void ApplyLevitationUpgrade()
    {
        int level = magicUpgradesDB.levitationLevel;

        if (level + 1 < magicUpgradesDB.levitationDurationValues.Length)
        {
            magicUpgradesDB.levitationLevel++;

            int newLevel = magicUpgradesDB.levitationLevel;

            levitationMagicData.levitationDuration = magicUpgradesDB.levitationDurationValues[newLevel];
            levitationMagicData.sphereFinalScale = magicUpgradesDB.sphereFinalScaleValues[newLevel];
        }
    }

    // HELPERS FOR THE UPGRADE MENU

    public bool IsPlayerUpgradeMaxed(PlayerUpgradeType type)
    {
        switch (type)
        {
            case PlayerUpgradeType.MaxHealth:
                return playerUpgradesDB.maxHealthLevel >= playerUpgradesDB.maxHealthValues.Length;

            case PlayerUpgradeType.MovementSpeed:
                return playerUpgradesDB.movementSpeedLevel >= playerUpgradesDB.movementSpeedValues.Length;

            case PlayerUpgradeType.ShotgunDamage:
                return playerUpgradesDB.shotgunDamageLevel >= playerUpgradesDB.shotgunMaxDamageValues.Length;
        }

        return true;
    }

    public bool IsConquestUpgradeMaxed(ConquestUpgradeType type)
    {
        switch (type)
        {
            case ConquestUpgradeType.Levitation:
                return magicUpgradesDB.levitationLevel >= magicUpgradesDB.levitationDurationValues.Length;
        }

        return true;
    }
}
