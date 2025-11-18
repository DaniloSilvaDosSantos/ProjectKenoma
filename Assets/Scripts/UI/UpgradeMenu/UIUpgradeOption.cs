using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIUpgradeOption : MonoBehaviour
{
    [Header("UI")]
    public Text titleText;
    public Button button;

    private UnityAction onClickCallback;

    public void Setup(string title, UnityAction callback)
    {
        titleText.text = title;

        onClickCallback = callback;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickCallback?.Invoke());
    }
}

