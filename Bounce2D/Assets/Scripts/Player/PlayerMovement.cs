using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // left/right variables
    private Rigidbody2D rb;
    public float MoveSpeed = 3;
    private bool MoveLeft;
    private bool MoveRight;

    // jump variables    
    private bool IsGrounded;
    public Transform FeetPos;
    public float CheckRadius;
    public LayerMask WhatIsGround;
    public float JumpForce;
    private float JumpTimeCounter;
    public float JumpTime;
    private bool IsJumping;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {        
        IsGrounded = Physics2D.OverlapCircle(FeetPos.position, CheckRadius, WhatIsGround);
        // left/right movement
        if(Input.GetKey("d"))
        {
            rb.AddForce(new Vector2(MoveSpeed, 0));
        }
        else if(Input.GetKey("a"))
        {
            rb.AddForce(new Vector2(-MoveSpeed, 0));
        }

        // jump
        // if(Input.GetKeyDown(KeyCode.Space) && IsGrounded == true)
        // {
        //     IsJumping = true;
        //     JumpTimeCounter = JumpTime;
        //     // rb.velocity = Vector2.up * JumpForce;
        //     rb.AddForce(new Vector2(0, JumpForce));
        //     Debug.Log("First jump");
        // }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            JumpTimeCounter = JumpTime;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            if(JumpTimeCounter > 0) 
            {
                // rb.velocity = Vector2.up * JumpForce;
                rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Force);
                Debug.Log("Continued jump");
                JumpTimeCounter  -= Time.deltaTime;
            }
            // else 
            // {
            //     IsJumping = false;
            // }
        }

        // // if(Input.GetKeyUp(KeyCode.Space))
        // // {
        // //     IsJumping = false;
        // // }
    }


    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == Tags.LeftWallTag)
        {
            rb.AddForce(new Vector2(JumpForce, 0), ForceMode2D.Impulse);
        }
        if(other.gameObject.tag == Tags.RightWallTag)
        {
            rb.AddForce(new Vector2(-JumpForce, 0), ForceMode2D.Impulse);
        }
        if(other.gameObject.tag == Tags.Ceiling)
        {
            Debug.Log("hit detected");
            rb.AddForce(new Vector2(0, -JumpForce), ForceMode2D.Impulse);
        }
    }
    
}
