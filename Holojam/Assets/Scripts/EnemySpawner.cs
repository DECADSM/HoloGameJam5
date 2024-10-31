using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] GameObject Melee;
    [SerializeField] GameObject Ranged;

    [Space(10)]
    [Header("Number of Enemies to Spawn")]
    [SerializeField] int NumOfMelee;
    [SerializeField] int NumOfRanged;

    private int MaxNumOfMelee;
    private int MaxNumOfRanged;

    [Space(5)]
    [Header("Spawning Interval")]
    [SerializeField] float SpawnerInterval;
    float SpawnerIntervalRunning;

    [Space(2)]
    [Header("Misc")]
    [SerializeField] bool Alternate;
    [Tooltip("Will start with all Melee first, If Alternate is on, Melee will start first")]
    [SerializeField] bool MeleeFirst;
    [Tooltip("Will start with all Ranged first, If Alternate is on, Ranged will start first")]
    [SerializeField] bool RangedFirst;

    enum ET
    {
        None,
        Melee,
        Ranged
    }
    private ET lastEnemySpawned = ET.None;
    void Start()
    {
        MaxNumOfMelee = NumOfMelee; 
        MaxNumOfRanged = NumOfRanged;
    }
    public void Reset()
    {
        NumOfMelee = MaxNumOfMelee;
        NumOfRanged = MaxNumOfRanged;
    }
    // Update is called once per frame
    void Update()
    {
        if (SpawnerIntervalRunning > 0)
            SpawnerIntervalRunning -= Time.deltaTime;

        if(SpawnerIntervalRunning <= 0)
        {
            if(MeleeFirst)
            {
                if (lastEnemySpawned == ET.None)
                {
                    lastEnemySpawned = ET.Melee;
                    Instantiate(Melee, transform.position, new Quaternion());
                    NumOfMelee--;
                }
                else if(!Alternate)
                {
                    if (NumOfMelee > 0)
                    {
                        Instantiate(Melee, transform.position, new Quaternion());
                        NumOfMelee--;
                    }
                    else if(NumOfMelee <= 0 && NumOfRanged > 0)
                    {
                        Instantiate(Ranged, transform.position, new Quaternion());
                        NumOfRanged--;
                    }
                }
                else if(Alternate)
                {
                    if (lastEnemySpawned == ET.Melee && NumOfRanged > 0)
                    {
                        lastEnemySpawned = ET.Ranged;
                        Instantiate(Ranged, transform.position, new Quaternion());
                        NumOfRanged--;
                    }
                    else if (lastEnemySpawned == ET.Ranged && NumOfMelee > 0)
                    {
                        lastEnemySpawned = ET.Melee;
                        Instantiate(Melee, transform.position, new Quaternion());
                        NumOfMelee--;
                    }
                    else if (NumOfMelee <= 0 || NumOfRanged <= 0)
                        Alternate = false;
                }
            }
            else if (RangedFirst)
            {
                if (lastEnemySpawned == ET.None)
                {
                    lastEnemySpawned = ET.Ranged;
                    Instantiate(Ranged, transform.position, new Quaternion());
                    NumOfRanged--;
                }
                else if (!Alternate)
                {
                    if (NumOfRanged > 0)
                    {
                        Instantiate(Ranged, transform.position, new Quaternion());
                        NumOfRanged--;
                    }
                    else if (NumOfRanged <= 0 && NumOfMelee > 0)
                    {
                        Instantiate(Melee, transform.position, new Quaternion());
                        NumOfMelee--;
                    }
                }
                else if (Alternate)
                {
                    if (lastEnemySpawned == ET.Ranged && NumOfMelee > 0)
                    {
                        lastEnemySpawned = ET.Melee;
                        Instantiate(Melee, transform.position, new Quaternion());
                        NumOfMelee--;
                    }
                    else if (lastEnemySpawned == ET.Melee && NumOfRanged > 0)
                    {
                        lastEnemySpawned = ET.Ranged;
                        Instantiate(Ranged, transform.position, new Quaternion());
                        NumOfRanged--;
                    }
                    else if (NumOfMelee <= 0 || NumOfRanged <= 0)
                        Alternate = false;
                }
            }
        }
    }
}
