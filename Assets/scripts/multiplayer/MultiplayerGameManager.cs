using UnityEngine;
using System.Collections;

public class MultiplayerGameManager : MonoBehaviour
{
    public static MultiplayerGameManager Instance { get; private set; }

    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Transform[] spawnPoints;

    [Header("Player 1 Controls")]
    public KeyCode[] player1Keys = new KeyCode[] { KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.Space };

    [Header("Player 2 Controls")]
    public KeyCode[] player2Keys = new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.Return };

    private MultiPlayerController player1;
    private MultiPlayerController player2;
    private bool gameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Set spawn points close together
        if (spawnPoints != null && spawnPoints.Length >= 2)
        {
            spawnPoints[0].position = new Vector3(-2f, 0f, 0f);
            spawnPoints[1].position = new Vector3(2f, 0f, 0f);
        }

        // Check if required components are assigned
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned in MultiplayerGameManager!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length < 2)
        {
            Debug.LogError("Spawn Points are not properly set up in MultiplayerGameManager! Need at least 2 spawn points.");
            return;
        }

        // Spawn Player 1
        GameObject player1Obj = Instantiate(playerPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
        player1 = player1Obj.GetComponent<MultiPlayerController>();
        if (player1 != null)
        {
            player1.moveLeftKey = player1Keys[0];
            player1.moveRightKey = player1Keys[1];
            player1.jumpKey = player1Keys[2];
            player1.attackKey = player1Keys[3];
            player1.playerName = "Player 1";
        }
        else
        {
            Debug.LogError("MultiPlayerController component not found on player prefab!");
        }

        // Spawn Player 2
        GameObject player2Obj = Instantiate(playerPrefab, spawnPoints[1].position, spawnPoints[1].rotation);
        player2 = player2Obj.GetComponent<MultiPlayerController>();
        if (player2 != null)
        {
            player2.moveLeftKey = player2Keys[0];
            player2.moveRightKey = player2Keys[1];
            player2.jumpKey = player2Keys[2];
            player2.attackKey = player2Keys[3];
            player2.playerName = "Player 2";
        }
        else
        {
            Debug.LogError("MultiPlayerController component not found on player prefab!");
        }

        // Flip Player 2 to face Player 1
        if (player2Obj != null)
        {
            Vector3 player2Scale = player2Obj.transform.localScale;
            player2Scale.x *= -1;
            player2Obj.transform.localScale = player2Scale;
        }
    }

    public void PlayerDied(MultiPlayerController deadPlayer)
    {
        if (gameOver) return;

        gameOver = true;
        MultiPlayerController winner = (deadPlayer == player1) ? player2 : player1;
        GameEndUI gameEndUI = FindObjectOfType<GameEndUI>();

        if (winner == player1 && gameEndUI != null)
            gameEndUI.ShowPlayer1Win();
        else if (winner == player2 && gameEndUI != null)
            gameEndUI.ShowPlayer2Win();

        // Show game over after 2 seconds
        if (gameEndUI != null)
            StartCoroutine(ShowGameOverAfterDelay(gameEndUI));
    }

    private IEnumerator ShowGameOverAfterDelay(GameEndUI gameEndUI)
    {
        yield return new WaitForSecondsRealtime(2f);
        gameEndUI.ShowGameOver();
    }
}