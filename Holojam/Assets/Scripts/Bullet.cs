using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public enum BulletType
{
    Gun,
    Bow,
    Magic,
};
public class Bullet : MonoBehaviour
{
    public float lifetime = 3f;
    public float speed = 100f;
    public int damage = 1;
    public int numPierce = 0;
    public BulletType type = BulletType.Gun;
    public GameObject parent;

    Animator animator;
    [SerializeField] AnimatorController AltareBolt;
    [SerializeField] AnimatorController HakkaSeal;
    [SerializeField] AnimatorController ShinriArrow;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        switch(type)
        {
            case BulletType.Gun:
                animator.runtimeAnimatorController = AltareBolt;
                break;
            case BulletType.Bow:
                animator.runtimeAnimatorController = ShinriArrow;
                break;
            case BulletType.Magic:
                animator.runtimeAnimatorController = HakkaSeal;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;

        if( lifetime <= 0 )
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && !collision.Equals(parent))
        {
            //TODO: damage the enemy
            numPierce--;
            collision.GetComponent<Enemy_Base>().TakeDamage(damage);
            if( numPierce < 0 )
                Destroy(gameObject);
        }
        else if(collision.CompareTag("Player") && !collision.Equals(parent))
        {
            collision.GetComponent<PlayerController>().RemoveHealth(damage);
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Shield"))
        {
            if(type != BulletType.Bow)
            {
                Destroy(gameObject);
                return;
            }
        }
        else if (collision.CompareTag("Boss") && !collision.Equals(parent))
        {
            collision.GetComponent<Enemy_Boss>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if((collision.CompareTag("Obstacle") && !collision.Equals(parent)))
        {
            Destroy(gameObject);
        }
        
    }
}
