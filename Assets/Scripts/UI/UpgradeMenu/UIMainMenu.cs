using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button returnButton;

    [Header("Options")]
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        playButton.onClick.AddListener(OnPlayPressed);
        optionsButton.onClick.AddListener(OnOptionsPressed);
        returnButton.onClick.AddListener(OnBackPressed);

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.6f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
    }

    private void OnPlayPressed()
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void OnOptionsPressed()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    private void OnBackPressed()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
}
