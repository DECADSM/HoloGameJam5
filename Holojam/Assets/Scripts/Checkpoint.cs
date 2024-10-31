using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform playerRespawnPos;
    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" )
        {
            if(playerRespawnPos == null)
            {
                playerRespawnPos = collision.transform;
                collision.GetComponent<PlayerController>().checkpoint = gameObject;
            }
        }
    }

    public void ResetLevel()
    {
        GameObject[] enemyArr = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] gamArr = GameObject.FindGameObjectsWithTag("Enemy Spawner");

        //clear all living enemies
        foreach( GameObject enemy in enemyArr)
        {
            GameObject.Destroy(enemy);
        }

        //reset the enemy spawners

        foreach (GameObject gam in gamArr)
        {
            gam.GetComponent<EnemySpawner>().Reset();
        }
    }
}
