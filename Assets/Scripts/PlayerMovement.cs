using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    //jump
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    private bool canDoubleJump = false;

    //wall
    private bool isWallSliding;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private Transform wallCheck;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private float Horizontal = 0f;

    


    private enum MovementState {idle, run, jump, fail }
    
    private void Awake()
    {
       rb = GetComponent<Rigidbody2D>();
       sprite = GetComponent<SpriteRenderer>();
       anim = GetComponent<Animator>();
       coll = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(Horizontal * moveSpeed, rb.velocity.y);

        if (IsGrounded())
        {
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true; // Reset double jump ability when grounded
            }
            else if (canDoubleJump) // Perform double jump
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
            else if (isWallSliding) // Perform wall jump
            {
                float wallJumpDirection = IsWalled() ? -Mathf.Sign(Horizontal) : 0f;
                rb.velocity = new Vector2(wallJumpDirection * jumpForce, jumpForce);
                isWallSliding = false; // Exit wall slide after wall jump
            }
        }

        UpdateAnimations();
        WallSlide();
    }
    private void UpdateAnimations()
    {
        MovementState state;
        if(Horizontal > 0f)
        {
            state = MovementState.run;
            sprite.flipX = false;
        }
        else if(Horizontal < 0f)
        {
            state = MovementState.run;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        if(rb.velocity.y > 1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -1f)
        {
            state = MovementState.fail;
        }
        anim.SetInteger("state", (int)state);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer); 
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
    }

    private void WallSlide()
    {
        if(IsWalled() && !IsGrounded() && Horizontal !=0f ) 
        { 
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else 
        { 
            isWallSliding = false;
        }
    }
}
