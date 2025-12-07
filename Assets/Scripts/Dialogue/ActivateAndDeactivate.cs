using UnityEngine;

public class ActivateAndDeactivate : MonoBehaviour
{
    public GameObject objectToBeActivated;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectToBeActivated != null)
            {
                objectToBeActivated.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}
