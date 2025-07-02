using UnityEngine;

public class BackgroundMusic2 : MonoBehaviour
{
    private static BackgroundMusic2 instance;

    void Awake()
    {
        // If another music manager exists, destroy it
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject); // Destroy the old one
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
