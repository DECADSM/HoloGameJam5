using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy_Base
{
    // Start is called before the first frame update
    float AttackRange = 15;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform BulletSpawnerTransform;

    
    protected override void Start()
    {
        base.Start();
        melee = false;
        attackTimerBase = 4;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(runningAttackTimer > 0)
        {
            runningAttackTimer -= Time.deltaTime;
        }

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
        if (distToPlayer <= 15)
            moving = false;
        else
            moving = true;
        //print(distToPlayer);
        if ((distToPlayer < 30 && trackingPlayer) && moving)
        {
            trackingPlayer = true;
            Vector2 player_X_only = new Vector2(player.transform.position.x, transform.position.y);

            if (player.transform.position.y > transform.position.y)
            {
                Node closestNode = FindClosestNode();
                if (closestNode != null)
                {
                    Jump();
                }
            }
            
            transform.position = Vector2.MoveTowards(transform.position, player_X_only, MoveSpeed * Time.deltaTime);
        }
        if (!moving)
            Attack();
    }

    protected override void Attack()
    {
        if(!melee)
        {
            
            if (runningAttackTimer <= 0)
            {
                Vector2 direction = (player.transform.position - BulletSpawnerTransform.position).normalized;
                
                GameObject b = Instantiate(Bullet, (direction * 1.5f) + new Vector2(transform.position.x, transform.position.y), new Quaternion());
                if (b != null)
                {
                    b.GetComponent<Bullet>().parent = gameObject;
                    Rigidbody2D bulletRB = b.GetComponent<Rigidbody2D>();
                    bulletRB.velocity = direction * b.GetComponent<Bullet>().speed;
                }
                else
                {
                    Debug.LogError("Failed to create a bullet");
                }
                runningAttackTimer = attackTimerBase;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Gizmos.DrawRay(transform.position, direction);
        
    }
}
