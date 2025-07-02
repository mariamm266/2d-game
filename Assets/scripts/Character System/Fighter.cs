using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(HitDetection))]
public class Fighter : MonoBehaviour
{
    [Header("Combat")]
    public float attackCooldown = 0.5f;
    public int baseDamage = 10;
    [SerializeField] protected float hitStunDuration = 0.3f; // Added hit stun duration
    
    protected float nextAttackTime;
    protected bool isAttacking;
    protected bool facingRight = true;
    protected float hitStunEndTime; // Track when hit stun ends

    [Header("Components")]
    protected Animator anim;
    protected Rigidbody2D rb;
    protected HitDetection hitDetection;
    protected Health health;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hitDetection = GetComponent<HitDetection>();
        health = GetComponent<Health>();
        
        // Subscribe to health damage event
        health.OnTakeDamage += HandleDamageTaken;
    }

    protected virtual void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (health != null)
            health.OnTakeDamage -= HandleDamageTaken;
    }

    protected virtual bool CanAttack()
    {
        return Time.time >= nextAttackTime && 
               !health.IsDead() && 
               !isAttacking &&
               Time.time >= hitStunEndTime; // Added hit stun check
    }

    // New method to handle damage taken
    protected virtual void HandleDamageTaken()
    {
        // Reset attack state when hit
        isAttacking = false;
        hitStunEndTime = Time.time + hitStunDuration;
        
        // Optional: Cancel current attack animation
        if (anim != null)
        {
            anim.ResetTrigger("Atk_1");        }
    }

    public virtual void BasicAttack_1()
    {
        if (!CanAttack()) return;
        
        isAttacking = true;
        anim.SetTrigger("Atk_1");
        nextAttackTime = Time.time + attackCooldown;
    }

    public virtual void BasicAttack_2()
    {
        if (!CanAttack()) return;
        
        isAttacking = true;
        anim.SetTrigger("Atk_2");
        nextAttackTime = Time.time + attackCooldown;
    }

    // Animation Events
    [System.Obsolete]
    public void OnAttack1Hit() => hitDetection.ActivateHitbox(0);
    [System.Obsolete]
    public void OnAttack2Hit() => hitDetection.ActivateHitbox(1);
    public void OnAttackEnd() => isAttacking = false;

    public void FlipCharacter(float direction)
    {
        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * (facingRight ? 1 : -1),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }
}