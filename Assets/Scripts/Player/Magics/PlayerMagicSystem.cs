using UnityEngine;
using System.Collections.Generic;

public class PlayerMagicSystem : MonoBehaviour
{
    [Header("References")]
    public PlayerController controller;
    [Space]
    [SerializeField] private DialogBoxSystem dialogBox;
    [SerializeField] private List<DialogData> dialogData = new List<DialogData>();
    [SerializeField] private MagicAuraColorController auraController;
    [SerializeField] private HUDUltimateReady ultimateHUD;

    
    [Header("Magics Controll Variables")]
    [SerializeField] private List<MagicData> unlockedMagics = new List<MagicData>();
    public IReadOnlyList<MagicData> UnlockedMagics => unlockedMagics;
    private Dictionary<MagicData, float> cooldownTimers = new Dictionary<MagicData, float>();
    private Dictionary<MagicData, int> killCounters = new Dictionary<MagicData, int>();

    [Header("Magic Selection")]
    [SerializeField] private int selectedMagic = 0;

    [SerializeField] private int minSelectedMagic = (int)MagicType.MagicLevitation;
    [SerializeField] private int maxSelectedMagic = (int)MagicType.MagicUltimate;

    private void Start()
    {
        controller = GetComponent<PlayerController>();

        dialogBox = FindAnyObjectByType<DialogBoxSystem>().GetComponent<DialogBoxSystem>();

        foreach (var magic in unlockedMagics)
        {
            cooldownTimers[magic] = 0f;
            killCounters[magic] = 0;
        }

        selectedMagic = FindNextUnlockedMagic(startIndex: minSelectedMagic, direction: +1);

        UpdateAuraColor();
    }

    private void Update()
    {
        UpdateCooldowns();
        UpdateAuraColor();
        UpdateUltimateHUD();

        HandleMagicSelectionScroll();

        if (Input.GetMouseButtonDown(1))
        {
            TryCastMagic((MagicType)selectedMagic);
        }
    }

    private void HandleMagicSelectionScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f) return;

        int direction = scroll > 0 ? +1 : -1;

        selectedMagic = FindNextUnlockedMagic(selectedMagic + direction, direction);

        UpdateAuraColor();
    }

    private int FindNextUnlockedMagic(int startIndex, int direction)
    {
        int index = startIndex;

        for (int i = 0; i < 10; i++)
        {
            if (index > maxSelectedMagic) index = minSelectedMagic;
            if (index < minSelectedMagic) index = maxSelectedMagic;

            MagicType type = (MagicType)index;

            MagicData magic = unlockedMagics.Find(m => m.type == type);
            if (magic != null)
            {
                return index;
            }

            index += direction;
        }

        return minSelectedMagic;
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

        //Debug.Log("Magic unlocked: " + magic.magicName);

        if(dialogBox != null) AwakeDialogBox(magic);
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
                //Debug.Log(magic.magicName + "still is in cooldown: " + cooldownTimers[magic]);
                controller.handAnimatorController.PlayFailedCast();
                return;
            }
        }

        if (magic.cooldownType == MagicCooldownType.KillCount)
        {
            if (killCounters[magic] < magic.killsRequired)
            {
                //int remaining = magic.killsRequired - killCounters[magic];
                //Debug.Log(magic.magicName + " requires "+ remaining +" more kills.");
                controller.handAnimatorController.PlayFailedCast();
                return;
            }
        }

        CastMagic(magic);
        UpdateAuraColor();

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
                controller.handAnimatorController.PlayCast();
                break;
            case MagicType.MagicAttraction:
                magicBehaviour = gameObject.AddComponent<MagicAttraction>();
                controller.handAnimatorController.PlayCast();
                break;
            case MagicType.MagicUltimate:
                magicBehaviour = gameObject.AddComponent<MagicUltimate>();
                controller.handAnimatorController.PlayUltimateCast();
                break;
            default:
                Debug.LogWarning("Magic type " + data.type + " unrecognized.");
                return;
        }

        magicBehaviour.Initialize(data, controller);
        magicBehaviour.Cast();
        Destroy(magicBehaviour, 0.1f);
    }

    private bool IsMagicReady(MagicData magic)
    {
        if (magic.cooldownType == MagicCooldownType.Time) return cooldownTimers[magic] <= 0f;

        if (magic.cooldownType == MagicCooldownType.KillCount) return killCounters[magic] >= magic.killsRequired;

        return false;
    }

    private void AwakeDialogBox(MagicData magic)
    {
        DialogData selectedDialog;

        if(magic.magicName == "Levitation")
        {
           selectedDialog = dialogData[(int)MagicType.MagicLevitation];
           dialogBox.StartDialog(selectedDialog); 
        }
        else if(magic.magicName == "Attraction")
        {
           selectedDialog = dialogData[(int)MagicType.MagicAttraction];
           dialogBox.StartDialog(selectedDialog); 
        }  
    }

    private void UpdateAuraColor()
    {
        if (auraController == null) return;

        MagicData magic = unlockedMagics.Find(m => m.type == (MagicType)selectedMagic);
        if (magic == null) return;

        bool isReady = IsMagicReady(magic);

        auraController.SetAuraReadyState(isReady, magic.magicColor);
    }

    private void UpdateUltimateHUD()
    {
        if (ultimateHUD == null) return;

        MagicData ultimate = unlockedMagics.Find(m => m.type == MagicType.MagicUltimate);

        if (ultimate == null)
        {
            ultimateHUD.SetHUDState(false);
            return;
        }

        bool ready = IsMagicReady(ultimate);

        ultimateHUD.SetHUDState(ready);
    }

}

