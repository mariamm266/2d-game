using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public bool isSinglePlayer = true;
    public int maxLevels = 5;
    public float matchDuration = 99f;

    [Header("Debug")]
    public int currentLevel = 1;
    public bool isPaused;
    public int player1Wins;
    public int player2Wins;

    public float matchTimer;
    private bool matchEnded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isSinglePlayer && !matchEnded)
        {
            matchTimer -= Time.deltaTime;
            if (matchTimer <= 0)
            {
                EndMatch("Time Over!");
            }
        }
    }

    public void StartGame(bool singlePlayer)
    {
        isSinglePlayer = singlePlayer;
        currentLevel = 1;
        player1Wins = 0;
        player2Wins = 0;

        if (isSinglePlayer)
            SceneManager.LoadScene("Level_1");
        else
            StartVersusMatch();
    }

    void StartVersusMatch()
    {
        matchTimer = matchDuration;
        matchEnded = false;
        SceneManager.LoadScene("VersusArena");
    }

    public void PlayerDefeated(int playerID)
    {
        if (matchEnded) return;

        if (playerID == 1) player2Wins++;
        else player1Wins++;

        string result = $"Player {playerID} KO!";
        EndMatch(result);
    }

    void EndMatch(string result)
    {
        matchEnded = true;
        Debug.Log(result + $" P1: {player1Wins} - P2: {player2Wins}");
        Invoke("LoadResultsScreen", 3f);
    }

    public void LevelComplete()
    {
        if (isSinglePlayer)
        {
            currentLevel++;
            if (currentLevel <= maxLevels)
                SceneManager.LoadScene("Level_" + currentLevel);
            else
                SceneManager.LoadScene("Victory");
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    void LoadResultsScreen()
    {
        SceneManager.LoadScene("Results");
    }

 public void GameOver()
    {
        if (isSinglePlayer)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            // For versus mode, treat player death as match end
            PlayerDefeated(gameObject.CompareTag("Player1") ? 1 : 2);
        }
    }

}