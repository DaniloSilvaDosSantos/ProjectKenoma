using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu: MonoBehaviour
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

        Radio.Instance.PlayMusic("Music/Menu", MusicTransition.Fade, 1f);
    
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnPlayPressed()
    {
        GameController.Instance.ChangeScene("Tutorial", true);

        Radio.Instance.StopMusic(true, 2.5f);
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
