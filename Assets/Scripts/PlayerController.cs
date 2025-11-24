using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public Vector2 currentVelocity;
    private FacingDirection facing = FacingDirection.right;
    private BoxCollider2D box;
    public LayerMask groundLayer;

    public float apexHeight = 1.25f;
    public float apexTime = 0.3f;
    public float terminalSpeed = 5f;
    public float coyoteTime = 0.1f;
    public float coyoteTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float xInput = 0f;

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
            float jumpVelocity = (2f * apexHeight) / apexTime;
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);

            coyoteTimer = 0f; //prevents double coyote jumps
        }
        else
        {
            rb.gravityScale = 1;
        }

        //checks if the terminal velocity has been reached. If so, it won't exceed it
        if (rb.linearVelocity.y < -terminalSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -terminalSpeed);
        }

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
}
