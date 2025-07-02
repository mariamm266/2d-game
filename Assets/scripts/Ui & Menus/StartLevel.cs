using System.Collections;
using UnityEngine;

public class LevelStartUI : MonoBehaviour
{
    public GameObject levelPanel;
    public float delayBeforeStart = 2f;
    public AudioSource startSound;
    public AudioSource backgroundMusic;

    private void Start()
    {
        StartCoroutine(ShowLevelPanel());
    }

    IEnumerator ShowLevelPanel()
    {
        Debug.Log("Showing level panel");
        levelPanel.SetActive(true);
        Time.timeScale = 0f;

        // Mute background music while panel is active
        if (backgroundMusic != null)
            backgroundMusic.mute = true;

        // Play start sound
        if (startSound != null && startSound.clip != null)
        {
            Debug.Log("Playing start sound");
            startSound.Play();
            yield return new WaitForSecondsRealtime(startSound.clip.length);
            startSound.Stop();
        }
        else
        {
            Debug.LogWarning("Start sound or clip is missing");
            yield return new WaitForSecondsRealtime(delayBeforeStart); // fallback delay
        }

        // Hide panel and resume game
        levelPanel.SetActive(false);
        Time.timeScale = 1f;

        if (backgroundMusic != null)
            backgroundMusic.mute = false;

        Debug.Log("Level started");
    }
}
