using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        //print(moveValue);
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
