using UnityEngine;
using System.Collections;

public class GameEndUI : MonoBehaviour
{
    public GameObject player1WinPanel;
    public GameObject player2WinPanel;
    public GameObject gameOverPanel;
    public AudioSource player1WinSound;  // Audio for Player 1 Win Panel
    public AudioSource player2WinSound;  // Audio for Player 2 Win Panel
    public AudioSource gameOverSound;    // Audio for Game Over Panel
    public AudioSource backgroundMusic;  // Background music to mute/unmute

    public void ShowPlayer1Win()
    {
        player1WinPanel.SetActive(true);
        if (backgroundMusic != null) backgroundMusic.Stop(); // Stop background music
        if (player1WinSound != null) player1WinSound.Play(); // Play win sound
        Time.timeScale = 0f;  // Pause game
    }

    public void ShowPlayer2Win()
    {
        player2WinPanel.SetActive(true);
        if (backgroundMusic != null) backgroundMusic.Stop(); // Stop background music
        if (player2WinSound != null) player2WinSound.Play(); // Play win sound
        Time.timeScale = 0f;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        if (backgroundMusic != null) backgroundMusic.Stop(); // Stop background music
        if (gameOverSound != null) gameOverSound.Play(); // Play game over sound
        Time.timeScale = 0f;
    }

    public void LoadNextLevelWithDelay(float delay)
    {
        StartCoroutine(LoadNextLevelAfterDelay(delay));
    }

    private System.Collections.IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        Time.timeScale = 1f; // Unpause before loading
        yield return new WaitForSecondsRealtime(delay);
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentScene + 1;
        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}