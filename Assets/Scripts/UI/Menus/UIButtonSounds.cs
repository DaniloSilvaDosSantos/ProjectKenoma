using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Radio.Instance != null)
        {
            Radio.Instance.PlaySFX("SFX/UIButtonHighlight");
        }
    }

    private void PlayClickSound()
    {
        if (Radio.Instance != null)
        {
            Radio.Instance.PlaySFX("SFX/UIButtonPressed");
        }
    }
}
