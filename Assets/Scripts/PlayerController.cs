using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    public enum CharacterState
    {
        Idle, Walking, Jumping, Dead
    }

    private CharacterState state = CharacterState.Idle;

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public Vector2 currentVelocity;
    private FacingDirection facing = FacingDirection.right;
    private BoxCollider2D box;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    //public float apexHeight = 1f;
   // public float apexTime = 0.2f;

    public float jumpForce = 8f;
    public float jumpCutMultiplier = 0.5f;
    public float terminalSpeed = 5f;
    public float coyoteTime = 0.1f;
    public float coyoteTimer = 0f;
    public float wallJumpPush = 10f;
    public float wallJumpForce = 12f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float xInput = 0f;
        bool touchingLeftWall = IsTouchingWallLeft();
        bool touchingRightWall = IsTouchingWallRight();

        bool onWall = touchingLeftWall || touchingRightWall;

        //keys for moving
        if (Input.GetKey(KeyCode.A))
        {
            xInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xInput = 1f;
        }    
        
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(xInput, 0f);
        MovementUpdate(playerInput);

        if (IsGrounded())
        {
            Debug.Log("true");
        }
        else
        {
            Debug.Log("false");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            
        }

        if (onWall && Input.GetButtonDown("Jump"))
        {
            int pushDir = touchingLeftWall ? 1 : -1;

            rb.linearVelocity = new Vector2(pushDir * wallJumpPush, wallJumpForce);
        }

        
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        //apply horizontal movement
        currentVelocity = new Vector2(playerInput.x * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = currentVelocity;

        //update facing direction
        if (playerInput.x < 0f)
        {
            facing = FacingDirection.left;
        }
            
        else if (playerInput.x > 0f)
        {
            facing = FacingDirection.right;
        }

        if (IsGrounded())
        {
            coyoteTimer = coyoteTime;  //reset when on the ground
        }
        else
        {
            coyoteTimer -= Time.deltaTime;  //count down when off the ground
        }

        //checks if the player pressed the space bar while also on the ground. this will make them jump
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (rb.linearVelocity.y > 0) //only cut off upward movement
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
        }

        //checks if the terminal velocity has been reached. If so, it won't exceed it
        if (rb.linearVelocity.y < -terminalSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -terminalSpeed);
        }

        //code I may switch back to but probably not
        //if (IsGrounded() && Input.GetButtonDown("Jump"))
        //{
        //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
       // }

       // if (Input.GetButtonUp("Jump"))
       // {
       //     if (rb.linearVelocity.y > 0)
       //     {
        //        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        //    }
       // }
    }

    public bool IsWalking()
    {
        if (currentVelocity.x == 0f) 
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public bool IsGrounded()
    {
        float extraHeight = 0.1f; //distance below feet to check

        //position slightly above the bottom of the player
        Vector2 origin = new Vector2(box.bounds.center.x, box.bounds.min.y + 0.01f);

        //thin horizontal box for feet
        Vector2 boxSize = new Vector2(box.bounds.size.x * 0.9f, 0.02f);

        //make the boxCast downward
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, extraHeight, groundLayer);

        if (hit.collider != null)
        {
            return true; //on ground
        }
        else
        {
            return false; //in air
        }
    }

    public FacingDirection GetFacingDirection()
    {
        if(currentVelocity.x < 0f)
        {
            return FacingDirection.left;
        }

        if (currentVelocity.x > 0f)
        {
            return FacingDirection.right;
        }

        return facing;

    }

    public bool IsTouchingWallLeft()
    {
        float extraWidth = 0.05f; //distance to check sideways

        Vector2 origin = new Vector2(box.bounds.min.x + 0.01f, box.bounds.center.y); //position slightly inside the player's left side

        Vector2 boxSize = new Vector2(0.02f, box.bounds.size.y * 0.9f); //tall vertical box for the side

        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.left, extraWidth, wallLayer);

        return hit.collider != null;
    }

    public bool IsTouchingWallRight()
    {
        float extraWidth = 0.05f;

        Vector2 origin = new Vector2(box.bounds.max.x - 0.01f, box.bounds.center.y); //position slightly inside the player's right side

        Vector2 boxSize = new Vector2(0.02f, box.bounds.size.y * 0.9f);

        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.right, extraWidth, wallLayer);

        return hit.collider != null;
    }

}
