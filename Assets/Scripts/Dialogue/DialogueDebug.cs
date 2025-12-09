using Unity.VisualScripting;
using UnityEngine;

public class DialogueDebug : MonoBehaviour
{
    [SerializeField] private DialogBoxSystem dialogBoxSystem;
    [SerializeField] private DialogData dialogData;
    [SerializeField] private KeyCode dialogKey = KeyCode.T;

    public void Update()
    {
        if (Input.GetKeyDown(dialogKey))
        {
            dialogBoxSystem.StartDialog(dialogData);
        }
    }
}
