using UnityEngine;

public abstract class MagicBase : MonoBehaviour
{
    [HideInInspector] public MagicData magicData;
    [HideInInspector] public PlayerController controller;
    [HideInInspector] public Camera playerCamera;

    public virtual void Initialize(MagicData data, PlayerController controller)
    {
        magicData = data;
        this.controller = controller;
        playerCamera = controller.playerCamera;
    }

    public abstract void Cast();
}

