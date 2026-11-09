
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int firstLevelBuildIndex = 1;
    [SerializeField] private bool loopLevels = false;

    public int CurrentLevelIndex =>
        SceneManager.GetActiveScene().buildIndex;

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

    public void NextLevel()
    {
        int nextLevel = CurrentLevelIndex + 1;

        if (nextLevel >= SceneManager.sceneCountInBuildSettings)
        {
            if (loopLevels)
            {
                LoadLevel(firstLevelBuildIndex);
            }
            else
            {
                Debug.Log("All levels completed!");
            }

            return;
        }

        LoadLevel(nextLevel);
    }

    public void RestartLevel()
    {
        LoadLevel(CurrentLevelIndex);
    }

    public void LoadLevel(int buildIndex)
    {
        if (buildIndex < 0 ||
            buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError(
                $"Invalid level build index: {buildIndex}"
            );
            return;
        }

        SceneManager.LoadScene(buildIndex);
    }

    public void LoadMainMenu()
    {
        LoadLevel(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
