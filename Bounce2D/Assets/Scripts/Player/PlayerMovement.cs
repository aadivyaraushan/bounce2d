using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // left/right variables
    private Rigidbody2D rb;
    public float MoveSpeed = 3;
    public float acceleration;
    public float deceleration;
    public float AirAccel;
    public float AirDecel;
    [Space(10)]
    public float FrictionAmount;
    [Space(10)]
    private Vector2 MoveInput;
    private Vector2 LastMoveInput;
    public float VelocityPower;

    // jump variables    
    private bool IsGrounded;
    public Transform FeetPos;
    public float CheckRadius;
    public LayerMask WhatIsGround;
    public float JumpForce;
    // public float JumpCutMultiplier;
    // public float JumpBufferTime;
    // public float FallGravityMultiplier;
    private float JumpTimeCounter;
    public float JumpTime;
    // public float LastGroundedTime = 0;
    // public float LastJumpTime = 0;
    // public bool JumpInputReleased = true;
    private bool IsJumping;
    // public float JumpCoyoteTime;
    public float BounceForce = 10f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {       

        IsGrounded = Physics2D.OverlapCircle(FeetPos.position, CheckRadius, WhatIsGround);
        // if(IsGrounded && !IsJumping)
        // {
        //     LastGroundedTime = JumpCoyoteTime; // resetting timer
        // }
        // #endregion

        #region Inputs
        MoveInput.x = Input.GetAxisRaw("Horizontal");
        MoveInput.y = Input.GetAxisRaw("Vertical");
        // if(Input.GetKey(KeyCode.Space))
        // {
        //     LastJumpTime = JumpBufferTime;
        // }
        // if(Input.GetKeyUp(KeyCode.Space))
        // {
        //     OnJumpUp();
        // }
        #endregion



        #region Movement

        if(rb.velocity.y > 0)
            IsJumping = true;
        
        if(MoveInput.x != 0)
            LastMoveInput.x = MoveInput.x;
        if(MoveInput.y != 0)
            LastMoveInput.y = MoveInput.y;

        // calculate the direction we want to move in and our desired velocity
        float TargetSpeed = MoveInput.x * MoveSpeed;

        // calculate difference between current velocity and desired velocity
        float SpeedDifference = TargetSpeed - rb.velocity.x;

        // change acceleration rate depending on situation
        float AccelerationRate = (Mathf.Abs(SpeedDifference) > 0.01f) ? acceleration : deceleration;

        // applies acceleration to speed difference, then raises to a set power so acceleration can increase with higher speeds.
        // finally multiplies by sign to reapply direction.
        float movement;
        if(!IsGrounded)
            movement = Mathf.Pow(TargetSpeed * AccelerationRate, VelocityPower);
        else
            movement = Mathf.Pow(Mathf.Abs(SpeedDifference) * AccelerationRate, VelocityPower) * Mathf.Sign(SpeedDifference);

        // applies force to rigidbody, multiplying by Vector2.right so that it only affects the X axis
        rb.AddForce(movement * Vector2.right);
        #endregion

        #region Friction
        // check if we're grounded and that we're trying to stop, not pressing backwards or forwards
        if (IsGrounded && Mathf.Abs(MoveInput.x) < 0.01f)
        {
            // then use either minimum friction amount or our velocity
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(FrictionAmount));
            // sets to movement direction
            amount *= Mathf.Sign(rb.velocity.x);
            // applies force against movement direction
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion

        #region Jump

        if(Input.GetKeyDown(KeyCode.Space) && IsJumping == false)
        {
            IsJumping = true;
            JumpTimeCounter = JumpTime;
            // rb.velocity = Vector2.up * JumpForce;
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            JumpTimeCounter = JumpTime;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            if(JumpTimeCounter > 0 && IsJumping == false ) 
            {
                IsJumping = true;
                // rb.velocity = Vector2.up * JumpForce;
                rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Force);
                Debug.Log("Continued jump");
                JumpTimeCounter  -= Time.deltaTime;
            }
            else 
            {
                IsJumping = false;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            IsJumping = false;
        }

        if(rb.velocity.y <= 0)
        {
            IsJumping = false;
        }
        #endregion
        // if(LastGroundedTime > 0 && LastJumpTime > 0 && !IsJumping) Jump();

        // if(LastJumpTime > 0 && !IsJumping && JumpInputReleased)
        // {
        //     if(LastGroundedTime > 0)
        //     {
        //         LastGroundedTime = 0;
        //         Jump();
        //     }
        // }

        // #endregion

        // #region Jump Gravity
        // if(rb.velocity.y < 0 && LastGroundedTime <= 0)
        // {
        //     rb.gravityScale = rb.gravityScale * FallGravityMultiplier;
        // }
        // else
        // {
        //     rb.gravityScale = rb.gravityScale;
        // }
        // #endregion

    }


    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        #region BounceOffWalls
        if(other.gameObject.tag == Tags.LeftWallTag)
        {
            rb.AddForce(new Vector2(BounceForce, 0), ForceMode2D.Impulse);
        }
        if(other.gameObject.tag == Tags.RightWallTag)
        {            
            rb.AddForce(new Vector2(-BounceForce, 0), ForceMode2D.Impulse);
        }
        if(other.gameObject.tag == Tags.Ceiling)
        {
            Debug.Log("hit detected");
            rb.AddForce(new Vector2(0, -BounceForce), ForceMode2D.Impulse);
        }
        #endregion

    }
}
    
//     private void Jump()
//     {
//         // apply force, using impulse force mode
//         rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
//         IsJumping = true;
//         LastGroundedTime = 0;
//         LastJumpTime = 0;
//         JumpInputReleased = false;
//     }

//     public void OnJump()
//     {
//         LastJumpTime = JumpBufferTime;
//         JumpInputReleased = false;
//     }
    
//     public void OnJumpUp()
//     {
//         // first condition checks if player is in air
//         // second condition checks if player is jumping
//         if(rb.velocity.y > 0 && IsJumping == true)
//         {
//             // reduces current y velocity by amount ( 0 - 1)
//             rb.AddForce(Vector2.down * rb.velocity.y * (1 - JumpCutMultiplier), ForceMode2D.Impulse);
//         }

//         JumpInputReleased = true;
//         LastJumpTime = 0;
//     }

// }
