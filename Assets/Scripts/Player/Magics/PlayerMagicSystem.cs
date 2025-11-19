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

    [Header("Inputs To Cast The Magics")]
    [SerializeField] private KeyCode inputLevitationMagic = KeyCode.Alpha1;

    private void Start()
    {
        controller = GetComponent<PlayerController>();

        foreach (var magic in unlockedMagics) cooldownTimers[magic] = 0f;
    }

    private void Update()
    {
        UpdateCooldowns();

        if (Input.GetKeyDown(inputLevitationMagic)) TryCastMagic(MagicType.MagicLevitation);
    }

    private void UpdateCooldowns()
    {
        var keys = new List<MagicData>(cooldownTimers.Keys);
        foreach (var key in keys)
        {
            if (cooldownTimers[key] > 0f) cooldownTimers[key] -= Time.deltaTime;
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

        if (!cooldownTimers.TryGetValue(magic, out float timer))
        {
            cooldownTimers[magic] = 0f;
            timer = 0f;
        }

        if (timer > 0f)
        {
            Debug.Log(magic.magicName + " still is in a cooldown, cooldown time: " + timer);
            return;
        }

        CastMagic(magic);
        cooldownTimers[magic] = magic.cooldown;
    }

    private void CastMagic(MagicData data)
    {
        MagicBase magicBehaviour;

        switch (data.type)
        {
            case MagicType.MagicLevitation:
                magicBehaviour = gameObject.AddComponent<MagicLevitation>();
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

