using System.Collections;
using System.Collections.Generic;
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
    public BulletType type = BulletType.Gun;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if(collision.tag != "Player")
        {
            //TODO: damage the enemy

            collision.GetComponent<Enemy_Base>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Player") && !collision.Equals(parent))
        {
            collision.GetComponent<PlayerController>().RemoveHealth(damage);
            Destroy(gameObject);
        }
    }
}
