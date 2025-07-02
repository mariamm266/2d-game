using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic backgroundMusic;
    void Awake()
    {
        if(backgroundMusic == null){
            backgroundMusic = this;
            DontDestroyOnLoad(backgroundMusic);
        }
        else{
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
