using UnityEngine;

public class MultiPlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float attackRange = 2f;
    public int maxHealth = 100;
    public int attackDamage = 20;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float attackDelay = 0.3f; // Delay before damage is dealt
    public float groundCheckRadius = 0.2f; // Increased radius for better ground detection
    public string playerName = "Player"; // Add player name

    [Header("Input Settings")]
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode attackKey = KeyCode.Space;

    private int currentHealth;
    private Animator animator;
    private bool isDead = false;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canJump = true;
    private float lastJumpTime;
    public float jumpCooldown = 1f; // Time between jumps
    private MultiplayerGameManager gameManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        gameManager = FindObjectOfType<MultiplayerGameManager>();

        // Configure Rigidbody2D for better platformer physics
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        if (isDead) return;

        // Check if grounded with improved detection
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, isGrounded ? Color.green : Color.red);

        // Movement
        float moveInput = 0f;
        if (Input.GetKey(moveLeftKey)) moveInput = -1f;
        if (Input.GetKey(moveRightKey)) moveInput = 1f;

        // Flip character based on movement direction
        if (moveInput > 0 && !isFacingRight) Flip();
        if (moveInput < 0 && isFacingRight) Flip();

        // Move the player using physics
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Set running animation
        animator.SetBool("IsRunning", Mathf.Abs(moveInput) > 0);

        // Jump
        if (Input.GetKeyDown(jumpKey) && isGrounded && canJump && Time.time >= lastJumpTime + jumpCooldown)
        {
            isJumping = true;
            canJump = false;
            lastJumpTime = Time.time;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vertical velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }

        // Reset jump state when grounded
        if (isGrounded)
        {
            isJumping = false;
            canJump = true;
            animator.SetBool("IsJumping", false);
        }

        // Attack
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Attack()
    {
        animator.SetTrigger("Atk_1");
        Invoke("DealDamage", attackDelay); // Delay the damage dealing
    }

    void DealDamage()
    {
        // Find nearby players and deal damage
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            MultiPlayerController otherPlayer = hitCollider.GetComponent<MultiPlayerController>();
            if (otherPlayer != null && otherPlayer != this)
            {
                otherPlayer.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}"); // Debug log
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player died!");
        animator.SetTrigger("death");

        // Stop all movement
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        // Notify game manager of death
        if (MultiplayerGameManager.Instance != null)
        {
            MultiplayerGameManager.Instance.PlayerDied(this);
        }

        // Disable the controller after a short delay to allow death animation to play
        Invoke("DisableController", 0.1f);
    }

    void DisableController()
    {
        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}