using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AwakeTheDialog : MonoBehaviour
{
    [SerializeField] private List<DialogData> dialogDataList = new List<DialogData>();
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private bool isRepeatable = false;
    [SerializeField] private float repeatCooldown = 15f;
    [SerializeField] private float delayDialog = 0.5f;

    private DialogBoxSystem dialogBox;
    private bool canTriggerAgain = true;
    private bool isPlayerInside = false;
    private Coroutine cooldownRoutine;

    void Start()
    {
        meshRenderer.enabled = false;
        dialogBox = FindAnyObjectByType<DialogBoxSystem>();
    }

    void AwakeDialog()
    {
        if (dialogDataList == null || dialogDataList.Count == 0) return;

        DialogData selectedDialog = dialogDataList[Random.Range(0, dialogDataList.Count)];
        dialogBox.StartDialog(selectedDialog);

        if (!isRepeatable)
        {
            gameObject.SetActive(false);
        }
        else
        {
            canTriggerAgain = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canTriggerAgain) return;
        if (!other.CompareTag("Player")) return;

        isPlayerInside = true;

        Invoke("AwakeDialog", delayDialog);
    }

    void OnTriggerExit(Collider other)
    {
        if (!isRepeatable) return;
        if (!other.CompareTag("Player")) return;

        isPlayerInside = false;

        if (cooldownRoutine == null)
        {
            cooldownRoutine = StartCoroutine(ResetTriggerAfterCooldown());
        }
    }

    private IEnumerator ResetTriggerAfterCooldown()
    {
        yield return new WaitForSeconds(repeatCooldown);

        if (!isPlayerInside)
        {
            canTriggerAgain = true;
        }

        cooldownRoutine = null;
    }
}

