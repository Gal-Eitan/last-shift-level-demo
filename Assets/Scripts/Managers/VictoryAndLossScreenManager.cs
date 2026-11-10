
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryAndLossScreenManager : MonoBehaviour
{
    [Header("Screen Scenes")]
    [SerializeField] private string victorySceneName = "Victory";
    [SerializeField] private string gameOverSceneName = "GameOver";

    private bool gameEnded = false;

    public void ShowVictoryScreen()
    {
        Debug.Log("VIctory screen");
        if (gameEnded) return;

        gameEnded = true;
        LoadScreen(victorySceneName);
    }

    public void ShowGameOverScreen()
    {
        if (gameEnded) return;

        gameEnded = true;
        LoadScreen(gameOverSceneName);
    }

    private void LoadScreen(string sceneName)
    {
        // Ensure the next scene isn't paused.
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }
}
