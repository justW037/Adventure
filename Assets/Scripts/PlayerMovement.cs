using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private float Horizontal = 0f;

    private enum MovementState {idle, run, jump, fail }
    
    private void Awake()
    {
       rb = GetComponent<Rigidbody2D>();
       sprite = GetComponent<SpriteRenderer>();
       anim = GetComponent<Animator>(); 
    }
    private void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(Horizontal * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
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
}
