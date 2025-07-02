using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Image SoundOnIcon;
    [SerializeField] Image SoundOffIcon;
    private bool muted = false;
    void Start()
    {
        if (!PlayerPrefs.HasKey("muted")) {
        PlayerPrefs.SetInt("muted", 0);
    }

    Load(); // Load the saved value

    // Apply loaded setting to AudioListener
    AudioListener.pause = muted;

    UpdateButtonIcon();
        
    }
    public void OnButtonPress(){
        if(muted == false){
            muted = true;
            AudioListener.pause = true;
        }
        else{
            muted = false;
            AudioListener.pause = false;
        }
        Save();
        UpdateButtonIcon();
    }
    private void UpdateButtonIcon(){
        if(muted == false){
            SoundOnIcon.enabled = true;
            SoundOffIcon.enabled = false;
        }
        else{
             SoundOnIcon.enabled = false;
            SoundOffIcon.enabled = true;
        }
    }
    
    private void Load(){
        muted = PlayerPrefs.GetInt("muted")==1;
    }

    private void Save(){
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
