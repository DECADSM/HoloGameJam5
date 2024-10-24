using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    [SerializeField] float Health;

    private SpriteRenderer SR;
    private Color OGColor;

    //Player
    [SerializeField] GameObject Player;
    PlayerController PC;

    [Space(5)]
    [Header("Boss Elements")]
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject Bullet;
    [SerializeField] BoxCollider2D StompCollider;

    // Start is called before the first frame update
    void Start()
    {
        PC = Player.GetComponent<PlayerController>();

        SR = GetComponent<SpriteRenderer>();
        OGColor = SR.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
        SR.color = Color.red;
        StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        yield return new WaitForSeconds(0.25f);
        SR.color = OGColor;
    }


    //Move Set
    private void DeployShield()
    {

    }

    void ShotgunShot()
    {

    }

    void Stomp()
    {

    }

    void Rush()
    {

    }

    void CorruptionState()
    {
        //Will be immune from all damage except from Hakka
    }

}
