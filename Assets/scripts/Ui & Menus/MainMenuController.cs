using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup OptionPanel;
    public CanvasGroup PlayMode;

    public void PlayMulti(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }
    public void PlayAdventure(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


    public void Option(){
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
    }
    public void Back(){
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
    }
    public void PlayModes(){
        PlayMode.alpha = 1;
        PlayMode.blocksRaycasts = true;
    }
    public void Back2(){
        PlayMode.alpha = 0;
        PlayMode.blocksRaycasts = false;
    }
    
    public void QuitGame(){
        Application.Quit();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
