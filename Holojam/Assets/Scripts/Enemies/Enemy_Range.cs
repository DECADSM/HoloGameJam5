using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy_Base
{
    // Start is called before the first frame update
    float AttackRange = 15;
    protected override void Start()
    {
        base.Start();
        melee = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
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
    }

    protected override void Attack()
    {

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
}
