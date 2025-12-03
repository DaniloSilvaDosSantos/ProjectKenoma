using UnityEngine;
using System.Collections.Generic;

public class PlayerMagicSystem : MonoBehaviour
{
    [Header("References")]
    public PlayerController controller;
    
    [Header("Magics Controll Variables")]
    [SerializeField] private List<MagicData> unlockedMagics = new List<MagicData>();
    public IReadOnlyList<MagicData> UnlockedMagics => unlockedMagics;
    private Dictionary<MagicData, float> cooldownTimers = new Dictionary<MagicData, float>();
    private Dictionary<MagicData, int> killCounters = new Dictionary<MagicData, int>();

    [Header("Inputs To Cast The Magics")]
    [SerializeField] private KeyCode inputLevitationMagic = KeyCode.Alpha1;
    [SerializeField] private KeyCode inputAttractionMagic = KeyCode.Alpha2;
    [SerializeField] private KeyCode inputUltimateMagic = KeyCode.Alpha3;

    private void Start()
    {
        controller = GetComponent<PlayerController>();

        foreach (var magic in unlockedMagics)
        {
            cooldownTimers[magic] = 0f;
            killCounters[magic] = 0;
        } 
    }

    private void Update()
    {
        UpdateCooldowns();

        if (Input.GetKeyDown(inputLevitationMagic)) TryCastMagic(MagicType.MagicLevitation);
        if (Input.GetKeyDown(inputAttractionMagic)) TryCastMagic(MagicType.MagicAttraction);
        if (Input.GetKeyDown(inputUltimateMagic)) TryCastMagic(MagicType.MagicUltimate);
    }

    private void UpdateCooldowns()
    {
        foreach (var magic in unlockedMagics)
        {
            if (magic.cooldownType == MagicCooldownType.Time)
            {
                if (cooldownTimers[magic] > 0f) cooldownTimers[magic] -= Time.deltaTime;
            }
        }
    }

    public void RegisterKill()
    {
        foreach (var magic in unlockedMagics)
        {
            if (magic.cooldownType != MagicCooldownType.KillCount) continue;

            if (killCounters[magic] <= magic.killsRequired) killCounters[magic]++;
        }
    }

    public void UnlockMagic(MagicData magic)
    {
        if (magic == null) return;

        if (!unlockedMagics.Contains(magic)) unlockedMagics.Add(magic);

        if (!cooldownTimers.ContainsKey(magic)) cooldownTimers[magic] = 0f;

        Debug.Log("Magic unlocked: " + magic.magicName);
    }

    public void LockMagic(MagicData magic)
    {
        if (magic == null) return;

        unlockedMagics.Remove(magic);
        cooldownTimers.Remove(magic);
        killCounters.Remove(magic);
    }

    public bool IsUnlocked(MagicData magic)
    {
        return magic != null && unlockedMagics.Contains(magic);
    }

    private void TryCastMagic(MagicType type)
    {
        MagicData magic = unlockedMagics.Find(m => m.type == type);
        if (magic == null)
        {
            Debug.LogWarning("None magic of the type " + type + " has been unlocked!");
            return;
        }

        if (magic.cooldownType == MagicCooldownType.Time)
        {
            if (cooldownTimers[magic] > 0f)
            {
                Debug.Log(magic.magicName + "still is in cooldown: " + cooldownTimers[magic]);
                return;
            }
        }

        if (magic.cooldownType == MagicCooldownType.KillCount)
        {
            if (killCounters[magic] < magic.killsRequired)
            {
                int remaining = magic.killsRequired - killCounters[magic];
                Debug.Log(magic.magicName + " requires "+ remaining +" more kills.");
                return;
            }
        }

        CastMagic(magic);

        if (magic.cooldownType == MagicCooldownType.Time)
        {
            cooldownTimers[magic] = magic.cooldown;
        }
        else if (magic.cooldownType == MagicCooldownType.KillCount)
        {
            killCounters[magic] = 0;
        }
    }

    private void CastMagic(MagicData data)
    {
        MagicBase magicBehaviour;

        switch (data.type)
        {
            case MagicType.MagicLevitation:
                magicBehaviour = gameObject.AddComponent<MagicLevitation>();
                break;
            case MagicType.MagicAttraction:
                magicBehaviour = gameObject.AddComponent<MagicAttraction>();
                break;
            case MagicType.MagicUltimate:
                magicBehaviour = gameObject.AddComponent<MagicUltimate>();
                break;
            default:
                Debug.LogWarning("Magic type " + data.type + " unrecognized.");
                return;
        }

        magicBehaviour.Initialize(data, controller);
        magicBehaviour.Cast();
        Destroy(magicBehaviour, 0.1f);
    }

}

