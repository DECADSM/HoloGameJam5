using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    [SerializeField]
    private Transform BulletSpawnerTransform;
    
    private InputAction ShootAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
