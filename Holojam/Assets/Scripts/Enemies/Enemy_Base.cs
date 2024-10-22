using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] bool moving = false;

    public Vector2[] PatrolPoints;
    Queue<Node> closedList;
    Node previous;
    [SerializeField]bool grounded = true;

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
            if(!CheckLOS())
                pf.PathFinding();
            closedList = new Queue<Node>(pf.ClosedList);
            closedList.Dequeue(); //This gets rid of the first node which is the enemy
        }
        if (Input.GetKeyDown(KeyCode.L))
            Jump();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1);
        if (hit.collider != null)
        {
            if (hit.collider.name.Contains("Platform") || hit.collider.name.Contains("Floor"))
            {
                grounded = true;
            }
        }
        //if (CheckLOS() && moving == true)
        if (moving)
        {
            if (closedList.Count > 0 && !CheckLOS())
            {
                float dist = Vector2.Distance(transform.position, closedList.Peek().transform.position);
                if (dist > 3)
                {
                    
                    if (closedList.Peek().transform.position.y > transform.position.y || previous.name.Contains("Left"))
                    { 
                            Jump();
                    }
                    transform.position = Vector2.MoveTowards(transform.position, closedList.Peek().transform.position, MoveSpeed * Time.deltaTime);
                }
                else
                {
                    previous = closedList.Peek();
                    closedList.Dequeue();
                }
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, MoveSpeed * Time.deltaTime);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (player != null)
        {
            if (collision.collider.gameObject.Equals(player.gameObject))
            {
                moving = false;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (player != null)
        {
            if (collision.collider.gameObject.Equals(player.gameObject))
            {
                moving = true;
            }
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
        if (grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpStrength);
            grounded = false;
        }
    }
}
