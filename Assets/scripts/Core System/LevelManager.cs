using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [System.Serializable]
    public class LevelSettings
    {
        public int enemyHealth = 100;
        public int enemyDamage = 10;
        public float enemySpeed = 3f;
        public int enemyCount = 1;
        public GameObject[] hazards;
    }

    [Header("Level Design")]
    public LevelSettings[] levels;

    [Header("References")]
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

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
        if (!GameManager.Instance.isSinglePlayer) return;
        SetupLevel(GameManager.Instance.currentLevel - 1);
    }


    void SetupLevel(int levelIndex)
    {
        levelIndex = Mathf.Clamp(levelIndex, 0, levels.Length - 1);
        LevelSettings settings = levels[levelIndex];

        // Spawn enemies
        for (int i = 0; i < settings.enemyCount; i++)
        {
            GameObject enemy = Instantiate(
                enemyPrefab,
                spawnPoints[i % spawnPoints.Length].position,
                Quaternion.identity
            );

            ConfigureEnemy(enemy, settings);
        }

        // Activate hazards
        foreach (GameObject hazard in settings.hazards)
        {
            hazard.SetActive(true);
        }
    }

    void ConfigureEnemy(GameObject enemy, LevelSettings settings)
    {
        Health health = enemy.GetComponent<Health>();
        health.maxHealth = settings.enemyHealth;

        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        ai.moveSpeed = settings.enemySpeed;

        Fighter fighter = enemy.GetComponent<Fighter>();
        fighter.baseDamage = settings.enemyDamage;
    }

    public void OnEnemyDefeated()
    {
        // Check if all enemies are defeated
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            GameManager.Instance.LevelComplete();
        }
    }
}