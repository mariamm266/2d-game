using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundBinder : MonoBehaviour
{
    void Start()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => {
                SFXManager.instance.PlayClick();
            });
        }
    }
}
