using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 



public class Menu : MonoBehaviour
{
    [SerializeField] GameObject PauseMenuUI;
    private bool IsPaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
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
        IsPaused= false;
        PauseMenuUI.SetActive(false);

    }
    public void Pause(){
        Time.timeScale = 0;
        IsPaused = true;
        PauseMenuUI.SetActive(true);
        
    }
    public void Restart(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Exit(){
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
