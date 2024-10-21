using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;
    


    public float jumpStrength = 5f;
    public float moveSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }
    //TODO:
    //make gravity scale change based on if player is holding jump to allow for variable jump height
    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //get movement as (1,0) or (0,0) or (-1,0)
        Vector2 moveValue = new Vector2(moveAction.ReadValue<Vector2>().x,0).normalized;
        //print(moveValue);

        //get make the player face the correct directionf
        if (moveValue.magnitude > 0f )
        {
            transform.localScale = new Vector3(moveValue.x, 1, 1);
        }
        // your movement code here
        rb.velocity = new Vector2(moveValue.x * moveSpeed * Time.deltaTime, rb.velocity.y);
        if (jumpAction.IsPressed() && rb.velocity.y == 0) //wont work if jumping into a ceiling so i should fix this later to be a ray cast
        {
           // print("jump");
            // your jump code here
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        }
    }

    
}
