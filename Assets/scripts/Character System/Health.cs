using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(Collider2D), typeof(Rigidbody2D))]
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public float invulnerabilityTime = 0.5f;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Death Settings")]
    public GameObject deathEffect;
    public string deathAnimationTrigger = "death";
    public float deathDelay = 2f;

    [Header("Level Transition")]
    public float levelLoadDelay = 1f;

    [Header("Player Death")]
    public string mainMenuSceneName = "MainMenu";
    public float mainMenuLoadDelay = 2f;

    [Header("Audio")]
    public AudioClip hurtSound;
    public AudioClip deathSound;

    private float lastHitTime;
    private Animator anim;
    private bool isDead = false;
    public event System.Action OnTakeDamage;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    [System.Obsolete]
    public bool TakeDamage(int damage)
    {
        if (isDead || IsInvulnerable()) return false;

        currentHealth -= damage;
        lastHitTime = Time.time;
        UpdateHealthUI();

        OnTakeDamage?.Invoke();

        if (hurtSound != null)
            AudioSource.PlayClipAtPoint(hurtSound, transform.position);

        if (anim != null)
            anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
            Die();

        return true;
    }

    [System.Obsolete]
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Disable physics
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // Disable collisions
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        // Play death animation
        if (anim != null)
            anim.SetTrigger(deathAnimationTrigger);

        // Effects
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        // إذا العدو مات → اللاعب كسب → ننتقل لليفل الجديد
        if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy died - starting win sequence");
            GameEndUI gameEndUI = FindObjectOfType<GameEndUI>();
            if (gameEndUI != null)
            {
                Debug.Log("Found GameEndUI - showing win panel");
                gameEndUI.ShowPlayer1Win();
                gameEndUI.LoadNextLevelWithDelay(2f);
            }
            else
            {
                Debug.LogError("GameEndUI not found in scene!");
            }
        }
        // إذا اللاعب مات → اللاعب التاني كسب → نعرض جيم أوفر
        else if (gameObject.CompareTag("Player"))
        {
            GameEndUI gameEndUI = FindObjectOfType<GameEndUI>();
            if (gameEndUI != null)
            {
                gameEndUI.ShowPlayer2Win();
                // Wait for 2 seconds before showing game over
                StartCoroutine(ShowGameOverAfterDelay());
            }
        }

        Destroy(gameObject, deathDelay);
    }

    private IEnumerator ShowGameOverAfterDelay()
    {
        float timer = 0f;
        while (timer < 2f)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        GameEndUI gameEndUI = FindObjectOfType<GameEndUI>();
        if (gameEndUI != null)
        {
            if (gameEndUI.player2WinPanel != null)
                gameEndUI.player2WinPanel.SetActive(false); // Hide LOSER panel
            gameEndUI.ShowGameOver();
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0); // Load main menu (index 0)
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = (float)currentHealth / maxHealth;
    }

    public bool IsDead() => isDead;
    public bool IsInvulnerable() => Time.time < lastHitTime + invulnerabilityTime;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
