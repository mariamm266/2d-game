using UnityEngine;

public class PlayerController : Fighter
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 12f;
    public LayerMask groundLayer;

    [Header("Input")]
    public string horizontalAxis = "Horizontal_P1";
    public string jumpButton = "Jump_P1";
    public string attackButton_1 = "Attack_P1";
    public string attackButton_2 = "Attack_P1_2";

    private BoxCollider2D col;
    private bool isGrounded;

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (health.IsDead()) return;
        
        HandleMovement();
        HandleJump();
        HandleAttacks();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis(horizontalAxis);
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        
        anim.SetBool("IsRunning", Mathf.Abs(moveInput) > 0.1f);
        FlipCharacter(moveInput);
    }

    private void HandleJump()
    {
        isGrounded = Physics2D.BoxCast(
            col.bounds.center, 
            col.bounds.size, 
            0f, 
            Vector2.down, 
            0.1f, 
            groundLayer
        );

        if (Input.GetButtonDown(jumpButton) && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        anim.SetBool("IsJumping", !isGrounded);
    }

    private void HandleAttacks()
    {
        if (Input.GetButtonDown(attackButton_1))
            BasicAttack_1();
            
        if (Input.GetButtonDown(attackButton_2))
            BasicAttack_2();
    }
}