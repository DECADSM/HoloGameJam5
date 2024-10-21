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
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
        playerTransform = GetComponentInParent<Transform>();
        Debug.Assert(playerTransform != null, "Gun failed to find parent transform");
       
        ShootAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if(ShootAction.WasPressedThisFrame())
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab,BulletSpawnerTransform.position,new Quaternion());
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = new Vector2(playerTransform.lossyScale.x, 0).normalized;
            print(direction);
            bulletRB.velocity = direction * bullet.GetComponent<Bullet>().speed;
        }
    }
}
