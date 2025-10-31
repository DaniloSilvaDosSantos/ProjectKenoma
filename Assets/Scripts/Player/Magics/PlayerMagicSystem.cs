using UnityEngine;
using System.Collections.Generic;

public class PlayerMagicSystem : MonoBehaviour
{
    [Header("References")]
    public PlayerController controller;
    
    [Header("Magics Controll Variables")]
    public List<MagicData> unlockedMagics = new List<MagicData>();
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
        foreach (var key in new List<MagicData>(cooldownTimers.Keys))
        {
            if (cooldownTimers[key] > 0) cooldownTimers[key] -= Time.deltaTime;
        }
    }

    private void TryCastMagic(MagicType type)
    {
        MagicData magic = unlockedMagics.Find(m => m.type == type);
        if (magic == null)
        {
            Debug.LogWarning("None magic of the type " + type + " has been unlocked!");
            return;
        }

        if (cooldownTimers[magic] > 0)
        {
            Debug.Log(magic.magicName + "still is in a cooldown, cooldown time: " + cooldownTimers[magic]);
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

