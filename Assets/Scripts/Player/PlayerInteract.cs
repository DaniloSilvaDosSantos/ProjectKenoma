using UnityEngine;

public class PlayerInteract: MonoBehaviour
{
    public float interactRange = 10f;
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                var interactable = hit.collider.GetComponentInParent<IInteractable>();

                Debug.Log(interactable);

                if (interactable != null)
                {
                    interactable.OnInteracted();
                }
            }
        }
    }
}

