using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text phraseText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Game Over Phrases")]
    [TextArea]
    public string[] phrases;

    private PlayerUpgradeManager upgradeManager;

    private void Awake()
    {
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.gameOverMenu = this;
        }
    }

    private void Start()
    {
        restartButton.onClick.AddListener(OnRestartPressed);
        mainMenuButton.onClick.AddListener(OnMainMenuPressed);

        upgradeManager = FindAnyObjectByType<PlayerUpgradeManager>();

        if (phrases != null && phrases.Length > 0)
        {
            phraseText.text = phrases[Random.Range(0, phrases.Length)];
        }
    }

    private void OnRestartPressed()
    {
        if (upgradeManager != null)
        {
            upgradeManager.ResetUpgrades();
        }
        else
        {
            Debug.Log("Player Upgrade System reference is missing!");
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnMainMenuPressed()
    {
        if (upgradeManager != null)
        {
            upgradeManager.ResetUpgrades();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

