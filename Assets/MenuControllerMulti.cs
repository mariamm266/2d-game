using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

public class MenuControllerMulti : MonoBehaviour
{
    [SerializeField] GameObject PauseMenuUI2;
    [SerializeField] AudioSource[] allAudioSources; // Array to store all active audio sources
    private bool IsPaused = false;
    private bool wereSoundsPlaying = false; // Track if sounds were playing before pause

    void Start()
    {
        PauseMenuUI2.SetActive(false);
        // Get all active audio sources in the scene at start
        allAudioSources = FindObjectsOfType<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(IsPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        Time.timeScale = 1;
        IsPaused = false;
        PauseMenuUI2.SetActive(false);
        
        // Restore audio if sounds were playing before pause
        if(wereSoundsPlaying)
        {
            UnmuteAllAudio();
        }
    }

    public void Pause(){
        Time.timeScale = 0;
        IsPaused = true;
        PauseMenuUI2.SetActive(true);
        
        // Check if any sounds are playing and mute them
        wereSoundsPlaying = AreAnyAudioSourcesPlaying();
        MuteAllAudio();
    }

    public void Restart(){
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    

    public void Exit(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    // Helper method to mute all audio sources
    private void MuteAllAudio()
    {
        allAudioSources = FindObjectsOfType<AudioSource>(); // Refresh audio sources
        foreach(AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = true;
        }
    }

    // Helper method to unmute all audio sources
    private void UnmuteAllAudio()
    {
        allAudioSources = FindObjectsOfType<AudioSource>(); // Refresh audio sources
        foreach(AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = false;
        }
    }

    // Helper method to check if any audio is playing
    private bool AreAnyAudioSourcesPlaying()
    {
        allAudioSources = FindObjectsOfType<AudioSource>(); // Refresh audio sources
        foreach(AudioSource audioSource in allAudioSources)
        {
            if(audioSource.isPlaying && !audioSource.mute)
            {
                return true;
            }
        }
        return false;
    }
}