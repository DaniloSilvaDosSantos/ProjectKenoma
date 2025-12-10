using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    [SerializeField] private Image FadeScreen;

    public string currentScene = "";
    public string sceneToChange = "";

    public bool Fade = false;
    public float fadeSpeedMultiplier = 2f;
    public int points = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        HandleFade();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentScene != "MainMenu")
            {
                ChangeScene("MainMenu");
            }
        }
    }

    public void OffFade()
    {
        Fade = false;
    }

    void HandleFade()
    {
        var tempColor = FadeScreen.color;

        if (tempColor.a < 0.05f) FadeScreen.enabled = false;
        else FadeScreen.enabled = true;

        if (Fade && tempColor.a < 1f)
        {
            tempColor.a = Mathf.Min(tempColor.a + 1f * fadeSpeedMultiplier * Time.deltaTime, 1f);
            FadeScreen.color = tempColor;
        }
        else if (!Fade && tempColor.a > 0f)
        {
            tempColor.a = Mathf.Max(tempColor.a - 1f * fadeSpeedMultiplier * Time.deltaTime, 0f);
            FadeScreen.color = tempColor;
        }
    }

    public void ChangeScene(string newScene, bool doFade = true)
    {
        if (doFade)Fade = true;

        sceneToChange = newScene;

        Invoke("DoTheSceneChange", 2f);
    }

    public void DoTheSceneChange()
    {
        SceneManager.LoadScene(sceneToChange, LoadSceneMode.Single);
        currentScene = sceneToChange;

        Invoke("OffFade", 2f);
    }

    public void LeaveGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
