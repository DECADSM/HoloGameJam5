using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public float Health, Damage;
    public float MoveSpeed;

    public PlayerController player;
    bool moving = true;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
