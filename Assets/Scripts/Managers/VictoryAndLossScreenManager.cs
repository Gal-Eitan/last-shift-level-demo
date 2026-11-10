
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryAndLossScreenManager : MonoBehaviour
{
    public static VictoryAndLossScreenManager Instance { get; private set; }

    [Header("Screen Scenes")]
    [SerializeField] private string victorySceneName = "VictoryScreen";
    [SerializeField] private string gameOverSceneName = "GameOverScreen";

    [Header("Level Progression")]
    [SerializeField, Min(1)] private int currentLevel = 1;

    public int CurrentLevel => currentLevel;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowVictoryScreen()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        LoadScreen(victorySceneName);
    }

    public void ShowGameOverScreen()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        LoadScreen(gameOverSceneName);
    }

    public void NextLevel()
    {
        currentLevel++;
        LoadCurrentLevel();
    }

    public void RestartLevel()
    {
        LoadCurrentLevel();
    }

    public void LoadCurrentLevel()
    {
        gameEnded = false;
        Time.timeScale = 1f;

        string levelSceneName = $"Level{currentLevel}";

        SceneManager.LoadScene(levelSceneName);
    }

    public void SetCurrentLevel(int level)
    {
        if (level < 1)
        {
            Debug.LogWarning("Level number must be at least 1.");
            return;
        }

        currentLevel = level;
    }

    public void ResetGameState()
    {
        gameEnded = false;
    }

    private void LoadScreen(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
