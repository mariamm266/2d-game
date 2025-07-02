using UnityEngine;
using System.Collections;

public class EnemyAI : Fighter 
{
    [Header("AI Settings")]
    public Transform player;
    public float moveSpeed = 5f;
    public float attackDistance = 1.5f; // Both stopping and attack distance
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Detection")]
    public float detectionRange = 5f;
    [Range(0, 1)] public float attackArcThreshold = 0.7f;
    
    private bool playerInRange = false;
    private bool isGrounded;
    private float lastAttackTime;

    protected override void Awake()
    {
        base.Awake();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null) Debug.LogError("Player not found!");
        }

        if (rb != null)
        {
            rb.gravityScale = 4f;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (player == null || GetComponent<Health>().IsDead()) return;
        
        CheckPlayerInRange();
        CheckGrounded();
        HandleAIBehavior();
    }

    void CheckPlayerInRange()
    {
        playerInRange = Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    void CheckGrounded()
    {
        var collider = GetComponent<Collider2D>();
        if (collider == null) return;

        Vector2 groundCheckPos = new Vector2(
            transform.position.x, 
            transform.position.y - collider.bounds.extents.y
        );
        
        isGrounded = Physics2D.Raycast(
            groundCheckPos, 
            Vector2.down, 
            groundCheckDistance, 
            groundLayer
        );
    }

void HandleAIBehavior()
{
    if (isAttacking || !playerInRange || GetComponent<Health>().IsDead()) return;

    float distance = Vector2.Distance(transform.position, player.position);
    
    // More forgiving stopping condition
    if (distance <= attackDistance * 1.1f) // 10% buffer
    {
        // Stop movement
        if (rb != null) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (anim != null) anim.SetBool("IsRunning", false);

        // Check if we should attack
        if (IsPlayerInAttackArc() && isGrounded && Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(AttackBehavior());
        }
    }
    else if (isGrounded)
    {
        MoveTowardsPlayer();
    }
}

    bool IsPlayerInAttackArc()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float dotProduct = Vector2.Dot(directionToPlayer, transform.right * transform.localScale.x);
        return dotProduct > attackArcThreshold;
    }

void MoveTowardsPlayer()
{
    Vector2 direction = (player.position - transform.position).normalized;
    float moveDirection = Mathf.Sign(direction.x);
    
    FlipCharacter(moveDirection);
    
    if (rb != null)
    {
        // Simple movement towards player with smoothing
        rb.linearVelocity = new Vector2(
            moveDirection * moveSpeed,
            rb.linearVelocity.y
        );
    }

    if (anim != null) anim.SetBool("IsRunning", true);
}

    IEnumerator AttackBehavior()
    {
        isAttacking = true;
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (anim != null) 
        {
            anim.SetBool("IsRunning", false);
            anim.SetTrigger("Atk_1");
        }
        
        BasicAttack_1();
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        // Draw attack arc
        if (player != null)
        {
            Gizmos.color = IsPlayerInAttackArc() ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}