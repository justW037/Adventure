using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    //jump
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    private bool canDoubleJump = false;


    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private float Horizontal = 0f;
    private bool isOnMovingPlatform = false;



    //wall
    private bool isWallSliding;
    [SerializeField] private float wallSlideSpeed;


    //sound
    [SerializeField] private AudioSource jumpSound;

    private enum MovementState {idle, run, jump, fail, wallSlide}
    
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
        if (IsGrounded() || isOnMovingPlatform)
        {
            canDoubleJump = true;
            isWallSliding = false;
        }
        if (IsWalled() && !IsGrounded())
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
       

        Jump();

        UpdateAnimations();
    }


    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {

            if (IsGrounded() || isOnMovingPlatform)
            {
                jumpSound.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true; // Reset double jump ability when grounded
            }
            else if (canDoubleJump) // Perform double jump
            {
                jumpSound.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }

        }
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
        else if (IsWalled() && !IsGrounded() && rb.velocity.y < -0f)
        {
            state = MovementState.wallSlide;
        }
        anim.SetInteger("state", (int)state);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer); 
    }

    private bool IsWalled()
    {
        bool isWalledRight = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.right, 0.1f, groundLayer);
        bool isWalledLeft = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.left, 0.1f, groundLayer);
        return isWalledRight || isWalledLeft;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isOnMovingPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isOnMovingPlatform = false;
        }
    }


}
