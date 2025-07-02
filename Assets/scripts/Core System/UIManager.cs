using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Game UI")]
    public Slider p1HealthBar;
    public Slider p2HealthBar;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI comboText;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject resultsScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
            pauseMenu.SetActive(GameManager.Instance.isPaused);
        }

        if (!GameManager.Instance.isSinglePlayer)
        {
            timerText.text = Mathf.Ceil(GameManager.Instance.matchTimer).ToString();
        }
    }

    public void UpdateHealth(int playerID, float current, float max)
    {
        if (playerID == 1) p1HealthBar.value = current / max;
        else p2HealthBar.value = current / max;
    }

    public void ShowCombo(string text)
    {
        comboText.text = text;
        comboText.GetComponent<Animator>().Play("ComboPopup");
    }

    public void ShowResults(int p1Wins, int p2Wins)
    {
        resultsScreen.SetActive(true);
        resultsScreen.transform.Find("ResultText").GetComponent<TextMeshProUGUI>().text = 
            $"{p1Wins} - {p2Wins}";
    }
}