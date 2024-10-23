using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Flying : Enemy_Base
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        float distToPlayer = CheckDistToPlayer();
        if (distToPlayer <= 15)
            moving = false;
        else
            moving = true;
        if ((distToPlayer < 30 || trackingPlayer) && moving)
        {
            trackingPlayer = true;
            Vector2 player_X_only = new Vector2(player.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, player_X_only, MoveSpeed * Time.deltaTime);
        }
    }
}
