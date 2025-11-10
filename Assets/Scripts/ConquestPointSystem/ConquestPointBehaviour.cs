using UnityEngine;

public class ConquestPointBehaviour : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Renderer conquestPointRenderer;
    [SerializeField] private Collider conquestPointCollider;

    private ConquestPointManager conquestPointManager;

    void Awake()
    {
        conquestPointRenderer = GetComponentInChildren<Renderer>();
        conquestPointCollider = GetComponentInChildren<Collider>();

        SetActiveVisual(false);
    }

    public void Init(ConquestPointManager manager)
    {
        conquestPointManager = manager;
    }

    public void SetActiveVisual(bool state)
    {
        if (conquestPointRenderer != null) conquestPointRenderer.enabled = state;
        if (conquestPointCollider != null) conquestPointCollider.enabled = state;
    }

    public void OnInteracted()
    {
        Debug.Log("Conquest Point Has Been Interacted.");

        conquestPointManager.PlayerInteractedWithPoint();
    }
}

