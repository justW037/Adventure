using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask jumpableGround;

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

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimations();
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
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround); //if player not on ground player can't jump
    }
}
