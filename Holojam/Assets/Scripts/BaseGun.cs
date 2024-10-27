using System;
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
    private bool bHasFired = false;

    [Space(10)]
    [Header("Character properties")]
    public float AltareChargeTime = 0f;
    public Color AltareColor = Color.white;
    public bool AltareFireOnFullCharge = true;

    [Space(10)]
    public float ShinriChargeTime = 0.5f;
    public Color ShinriColor = Color.red;
    public bool ShinriFireOnFullCharge = false;

    [Space(10)]
    public float HakkaChargeTime = 0.25f;
    public Color HakkaColor = Color.magenta;
    public bool HakkaFireOnFullCharge = true;

    [NonSerialized]
    public float currentChargeTime = 0f;
    private float currentMaxCharge = 0f;
    private bool FireOnFullCharge = false;
    // Start is called before the first frame update
    void Start()
    {
        SetCharacter(VTubeManNameGuy.Altare);
        playerTransform = GetComponentInParent<Transform>();
        Debug.Assert(playerTransform != null, "Gun failed to find parent transform");
       
        ShootAction = InputSystem.actions.FindAction("Shoot");
    }

    // Update is called once per frame
    void Update()
    {
        if(ShootAction.IsPressed() && !bHasFired)
        {
            currentChargeTime += Time.deltaTime;

            if(currentChargeTime >= currentMaxCharge && FireOnFullCharge)
            {
                CreateBullet();
            }
        }
        else if(ShootAction.WasReleasedThisFrame())
        {
            if (currentChargeTime >= currentMaxCharge && !FireOnFullCharge)
            {
                CreateBullet();
            }

            currentChargeTime = 0f;
            bHasFired = false;
        }

        
    }

    private GameObject CreateBullet()
    {
        GameObject projPrefab = null;
        switch (currentBullet)
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
        GameObject bullet =  GameObject.Instantiate(projPrefab, BulletSpawnerTransform.position, new Quaternion());

        if (bullet != null)
        {
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = new Vector2(playerTransform.lossyScale.x, 0).normalized;
            bulletRB.velocity = direction * bullet.GetComponent<Bullet>().speed;
        }
        else
        {
            Debug.LogError("Failed to create a bullet");
        }
        bHasFired = true;
        currentChargeTime = 0f;
        return bullet;
    }

    public void SetCharacter(VTubeManNameGuy guy)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        currentChargeTime = 0f;
        switch (guy)
        {
            case VTubeManNameGuy.Start:
                break;
            case VTubeManNameGuy.Altare:
                spriteRenderer.color = AltareColor;
                currentBullet = BulletType.Gun;
                currentMaxCharge = AltareChargeTime;     
                FireOnFullCharge = AltareFireOnFullCharge;
                break;
            case VTubeManNameGuy.Shinri:
                spriteRenderer.color = ShinriColor;
                currentBullet = BulletType.Bow;
                currentMaxCharge = ShinriChargeTime;
                FireOnFullCharge = ShinriFireOnFullCharge;
                break;
            case VTubeManNameGuy.Hakka:
                spriteRenderer.color = HakkaColor;
                currentBullet = BulletType.Magic;
                currentMaxCharge = HakkaChargeTime; 
                FireOnFullCharge = HakkaFireOnFullCharge;
                break;
            case VTubeManNameGuy.End:
                break;
            default:
                break;
        }
    }
}
