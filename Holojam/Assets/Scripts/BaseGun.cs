using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject ArrowPrefab;
    public GameObject magicSealPrefab;
    public BulletType currentBullet = BulletType.Gun;
    [SerializeField]
    private Transform BulletSpawnerTransform;
    
    private InputAction ShootAction;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
        playerTransform = GetComponentInParent<Transform>();
        Debug.Assert(playerTransform != null, "Gun failed to find parent transform");
       
        ShootAction = InputSystem.actions.FindAction("Shoot");
    }

    // Update is called once per frame
    void Update()
    {
        if(ShootAction.WasPressedThisFrame())
        {
            GameObject projPrefab = null;
            switch(currentBullet)
            {
                case BulletType.Gun:
                    projPrefab = bulletPrefab;
                    break;
                case BulletType.Bow:
                    projPrefab = ArrowPrefab;   
                    break;
                case BulletType.Magic:
                    projPrefab = magicSealPrefab;
                    break;

            }
            GameObject bullet = GameObject.Instantiate(projPrefab,BulletSpawnerTransform.position,new Quaternion());
            if(bullet != null)
            {
                Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                Vector2 direction = new Vector2(playerTransform.lossyScale.x, 0).normalized;
                bulletRB.velocity = direction * bullet.GetComponent<Bullet>().speed;
            }
            else
            {
                Debug.LogError("Failed to create a bullet");
            }
            
        }
    }
}
