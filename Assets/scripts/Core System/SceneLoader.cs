using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // Reset timescale
        SceneManager.LoadScene(sceneName);
    }

    public void StartSingleplayer()
    {
        GameManager.Instance.StartGame(true);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.battleMusic);
    }

    public void StartMultiplayer()
    {
        GameManager.Instance.StartGame(false);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.battleMusic);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}