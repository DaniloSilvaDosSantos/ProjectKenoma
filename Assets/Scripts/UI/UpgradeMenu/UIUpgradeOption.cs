using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIUpgradeOption : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI titleText;
    public Button upgradeButton;

    private UnityAction onClickCallback;

    public void Setup(string title, UnityAction callback)
    {
        titleText.text = title;

        onClickCallback = callback;

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => onClickCallback?.Invoke());
    }
}

