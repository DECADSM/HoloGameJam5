using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public float Health, Damage;
    public float MoveSpeed = 5, JumpStrength = 13;
    private Rigidbody2D rb;

    public float JumpDistance = 5;

    public PlayerController player;
    [SerializeField] public bool moving = false;

    [SerializeField] public Vector2[] PatrolPoints;
    [SerializeField] public bool grounded = true;
    public bool trackingPlayer = true;
    public bool melee = true;
    bool inMeleeRange = false;
    [SerializeField] BoxCollider2D AttackCollider;

    [NonSerialized] public float attackTimerBase = 3, runningAttackTimer = 0;

    //temp feedback
    [NonSerialized] public SpriteRenderer rend;
    [NonSerialized] public Color originalColor;

    List<Node> PlatformNodes;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        PatrolPoints = new Vector2[2];
        PatrolPoints[0] = transform.GetChild(0).position;
        PatrolPoints[1] = transform.GetChild(1).position;

        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        PlatformNodes = FindObjectsOfType<Node>().ToList();
        if(AttackCollider == null)
        {
            AttackCollider = GetComponentInChildren<BoxCollider2D>();
        }
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (runningAttackTimer > 0)
            runningAttackTimer -= Time.deltaTime;

        if (inMeleeRange)
            Attack();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                grounded = true;
            }
        }
        else
        {
            grounded = false;
        }
        float distToPlayer = CheckDistToPlayer();
        //print(distToPlayer);
        if ((distToPlayer < 20 || trackingPlayer) && moving)
        {
            trackingPlayer = true;
            Vector2 player_X_only = new Vector2(player.transform.position.x, transform.position.y);

            if(player.transform.position.y > transform.position.y)
            {
                Node closestNode = FindClosestNode();
                if(closestNode != null)
                {
                    Jump();
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, player_X_only, MoveSpeed * Time.deltaTime);
        }


        CheckDead();
    }

    protected virtual void Attack()
    {
        if(runningAttackTimer <= 0)
        {
            player.RemoveHealth(Damage);
            runningAttackTimer = attackTimerBase;
        }
    }

    public virtual void TakeDamage(float dmg)
    {
        Health -= dmg;
        rend.color = Color.blue;
        StartCoroutine(RevertColor());
    }

    private IEnumerator RevertColor()
    {
        yield return new WaitForSeconds(.25f);
        rend.color = originalColor;
    }


    void CheckDead()
    {
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector2.down * 1.5f);
        if (PatrolPoints.Length > 0)
        {
            Gizmos.DrawWireSphere(PatrolPoints[0], 1);
            Gizmos.DrawWireSphere(PatrolPoints[1], 1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            print("In Melee Range");
            inMeleeRange = true;
            moving = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        print("Exiting Melee Range");
        inMeleeRange = false;
        moving = true;
    }

    public Node FindClosestNode()
    {
        foreach (Node n in PlatformNodes)
        {
            float dist = Vector2.Distance(n.transform.position, transform.position);
            if (dist <= 5 && n.transform.position.y > transform.position.y)
            {
                //print(n.transform.parent.name + ": " + n.name + " " + dist);
                return n;
            }
        }
        return null;
    }

    public float CheckDistToPlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position);
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

    public virtual void Fly()
    {

    }
}
