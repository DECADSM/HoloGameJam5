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
    private ContactFilter2D wallContactFilter;
    private float capsuleCurveOffset = 0.75f;

    public float jumpStrength = 5f;
    public float moveSpeed = 50f;

    public float WallCheckCorrectionValue = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        LayerMask playerMask = 0b_1111_1111_1111_1111 ^  (1 << 3); //anything but a player
        wallContactFilter.SetLayerMask(playerMask);

    }
    //TODO:
    //make gravity scale change based on if player is holding jump to allow for variable jump height
    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        CapsuleCollider2D capsuleCollider = rb.GetComponent<CapsuleCollider2D>();

        //get movement as (1,0) or (0,0) or (-1,0)
        Vector2 moveValue = new Vector2(moveAction.ReadValue<Vector2>().x,0).normalized;
        //print(moveValue);

        //get make the player face the correct direction
        if (moveValue.magnitude > 0f )
        {
            transform.localScale = new Vector3(moveValue.x, 1, 1);
        }
        // check if moving into an object
        RaycastHit2D[] hits = new RaycastHit2D[10];
        int numHits = 0;
        numHits += Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y), moveValue, wallContactFilter, hits, capsuleCollider.bounds.extents.x + WallCheckCorrectionValue);
        numHits += Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y + capsuleCollider.bounds.extents.y * capsuleCurveOffset), moveValue, wallContactFilter, hits, capsuleCollider.bounds.extents.x + WallCheckCorrectionValue);
        numHits += Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y - capsuleCollider.bounds.extents.y * capsuleCurveOffset), moveValue, wallContactFilter, hits, capsuleCollider.bounds.extents.x + WallCheckCorrectionValue);

        if(numHits == 0) //if trying to move into a wall or some other object
        {
            rb.velocity = new Vector2(moveValue.x * moveSpeed * Time.deltaTime, rb.velocity.y);

        }

        

        if (jumpAction.IsPressed() && rb.velocity.y == 0) //wont work if jumping into a ceiling so i should fix this later to be a ray cast
        {
           // print("jump");
            // your jump code here
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        }
    }

    
}
