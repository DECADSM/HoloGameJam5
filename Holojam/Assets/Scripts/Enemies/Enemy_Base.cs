using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public float Health, Damage;
    public float MoveSpeed= 30, JumpStrength = 5;
    private Rigidbody2D rb;
    Pathfinding pf;

    public float JumpDistance = 5;

    public PlayerController player;
    bool moving = false;

    public Vector2[] PatrolPoints;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        pf = GetComponent<Pathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            pf.PathFinding();
        }
        //if(CheckLOS() && moving == true)
        if(moving)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, MoveSpeed * Time.deltaTime);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.Equals(player.gameObject))
        {
            moving = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.Equals(player.gameObject))
        {
            moving = true;
        }
    }

    public bool CheckLOS()
    {

        Vector2 DirectionToPlayer = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, DirectionToPlayer, 30);
        //Debug.DrawRay(transform.position, DirectionToPlayer, Color.cyan);
        if(hit)
        {
            if(hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    public virtual void Jump()
    {
        GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, JumpStrength);
    }
}
