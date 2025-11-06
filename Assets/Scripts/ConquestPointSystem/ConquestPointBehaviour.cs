using UnityEngine;

public class ConquestPointBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Collider col;

    private ConquestPointManager conquestPointManager;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        col = GetComponentInChildren<Collider>();

        SetActiveVisual(false);
    }

    public void Init(ConquestPointManager manager)
    {
        conquestPointManager = manager;
    }

    public void SetActiveVisual(bool state)
    {
        if (rend != null) rend.enabled = state;
        if (col != null) col.enabled = state;
    }

    public void OnInteracted()
    {
        conquestPointManager.PlayerInteractedWithPoint();
    }
}

