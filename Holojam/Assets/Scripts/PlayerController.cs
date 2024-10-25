using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum VTubeManNameGuy
{
    Start,
    Altare,
    Shinri,
    Hakka,
    End,
};
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction nextAction;
    private InputAction previousAction;
    private ContactFilter2D wallContactFilter;

    //temp player feedback
    private SpriteRenderer Character;
    private Color originalColor;
    //Actual player feedback -> trying to add knockback
    private bool hit = false;
    private float hitReset = 1, hitTimer;

    [Header("Stats")]
    [SerializeField] float Health = 100;
    [Space(10)]

    [Header("Movement Parameters")]
    public float jumpStrength = 5f;
    public float moveSpeed = 50f;

    [Space(10)]
    [Header("Wall Check Parameters")]
    public float WallCheckCorrectionValue = 0.1f;
    public LayerMask MoveThroughLayer = 1 << 3;

    [Space(10)]
    [Header("Character Parameters")]
    public VTubeManNameGuy currentCharacter = VTubeManNameGuy.Altare;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        nextAction = InputSystem.actions.FindAction("Next");
        previousAction = InputSystem.actions.FindAction("Previous");
        LayerMask playerMask = 0b_1111_1111_1111_1111 ^ MoveThroughLayer; //anything but a player
        wallContactFilter.SetLayerMask(playerMask);

        Character = GetComponent<SpriteRenderer>();
        originalColor = Character.color;
    }

    private void Update()
    {
        //wait for a little reset hit flag to move again to allow force/knockback to be added
        if (hitTimer > 0)
            hitTimer -= Time.deltaTime;
        if (hitTimer < 0)
            hit = false;

        if (nextAction.WasPressedThisFrame())
        {
            currentCharacter = currentCharacter + 1;
            if(currentCharacter == VTubeManNameGuy.End)
            {
                currentCharacter = VTubeManNameGuy.Altare;
            }
            UpdateCharacterProperties();
        }
        else if (previousAction.WasPressedThisFrame())
        {
            currentCharacter = currentCharacter - 1;
            if (currentCharacter == VTubeManNameGuy.Start)
            {
                currentCharacter = VTubeManNameGuy.Hakka;
            }
            UpdateCharacterProperties();
        }
    }
    //TODO:
    //make gravity scale change based on if player is holding jump to allow for variable jump height
    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        BoxCollider2D boxCollider = rb.GetComponent<BoxCollider2D>();

        //get movement as (1,0) or (0,0) or (-1,0)
        Vector2 moveValue = new Vector2(moveAction.ReadValue<Vector2>().x,0).normalized;
        //print(moveValue);

        //get make the player face the correct direction
        if (moveValue.magnitude > 0f )
        {
            transform.localScale = new Vector3(moveValue.x, 1, 1);
        }
        // check if moving into an object
        float halfSize = boxCollider.bounds.extents.x;
        RaycastHit2D[] hits = new RaycastHit2D[10];
        int numHits = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), new Vector2(0.9f, 1.9f), 0, moveValue, wallContactFilter, hits, halfSize+ WallCheckCorrectionValue);
        
        bool canmove = true;

        for (int i = 0; i < numHits; i++)
        {
            if (hits[i].transform.tag != "Enemy" && hits[i].distance < 0.1f)
            {
                canmove = false;
                //print(hits[i].distance);
                break;
            }
        }
        if (canmove && !hit) 
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

    private void UpdateCharacterProperties()
    {
        //do things like update sprites and playing a character swap animation, ect.
        //change current bullet
        BaseGun gun = GetComponentInChildren<BaseGun>();
        gun.currentBullet = (BulletType)(currentCharacter - 1);
    }

    void CheckDead()
    {
        if(Health <= 0)
        {

        }
    }

    public void RemoveHealth(float dmg)
    {
        Health -= dmg;
        Character.color = Color.red;
        StartCoroutine(RevertColor());
    }
    private IEnumerator RevertColor()
    {
        yield return new WaitForSeconds(.25f);
        Character.color = originalColor;
    }
    public void AddHealth(float amt)
    {
        Health += amt;
    }
    public void GettingHit()
    {
        hit = true;
        hitTimer = hitReset;
    }

}
