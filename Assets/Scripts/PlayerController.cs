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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        //apply horizontal movement
        currentVelocity = new Vector2(playerInput.x * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = currentVelocity;

        //update facing direction
        if (playerInput.x < 0)
        {
            facing = FacingDirection.left;
        }
            
        else if (playerInput.x > 0)
        {
            facing = FacingDirection.right;
        }
            
    }

    public bool IsWalking()
    {
        return Mathf.Abs(rb.linearVelocity.x) > 0.1f;
    }
    public bool IsGrounded()
    {
        return false;
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
