using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UITutorial : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private Button continueButton;

    [Header("Tutorial Texts")]
    [TextArea] public string text1;
    [TextArea] public string text2;
    [TextArea] public string text3;

    [SerializeField] private int currentIndex = 1;

    private void Start()
    {
        continueButton.onClick.AddListener(OnContinuePressed);
        ShowCurrentText();
    }

    private void ShowCurrentText()
    {
        switch (currentIndex)
        {
            case 1:
                tutorialText.text = text1;
                break;
            case 2:
                tutorialText.text = text2;
                break;
            case 3:
                tutorialText.text = text3;
                break;
        }
    }

    private void OnContinuePressed()
    {
        if (currentIndex < 3)
        {
            currentIndex++;
            ShowCurrentText();
        }
        else
        {
            SceneManager.LoadScene("TestScene");
        }
    }
}
