using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float Health;
    [SerializeField] float SlamDamage;
    [SerializeField] float ShieldTimer;
    float ShieldTimerRunning = 0;
    [SerializeField] float ShotTimer;
    float ShotTimerRunning = 0;
    [SerializeField] float NumOfShots;
    [SerializeField] float EnterCorruptionTimer;
    float EnterCorruptionTimerRunning = 0;
    [SerializeField] float CorruptionDuration;
    float CorruptionDurationRunning = 0;
    [SerializeField] float KnockbackStrength;

    [Space(5)]
    [Header("Flags")]
    [SerializeField] bool Corruption = false;

    private SpriteRenderer SR;
    private Color OGColor;

    //Player
    [Space(5)]
    [SerializeField] GameObject Player;
    PlayerController PC;

    [Space(5)]
    [Header("Boss Elements")]
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject Bullet;
    [SerializeField] BoxCollider2D StompCollider;
    [SerializeField] Transform BulletSpawner;

    int RandomAction = 0;

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
        if (ShieldTimerRunning > 0)
            ShieldTimerRunning -= Time.deltaTime;

        if (CorruptionDurationRunning > 0)
            CorruptionDurationRunning -= Time.deltaTime;

        if (ShotTimerRunning > 0)
            ShotTimerRunning -= Time.deltaTime;

        if (EnterCorruptionTimerRunning > 0)
            EnterCorruptionTimerRunning -= Time.deltaTime;

        RandomAction = Random.Range(1, 5);

        switch(RandomAction)
        {
            case 1://Shoot
                break;
            case 2://Rush
                break;
            case 3://Stomp
                break;
            case 4://Shield
                break;
            case 5://Corruption State
                break;
            default:
                break;
        }
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
    private void ShieldSwitch()
    {
        if (Shield.activeSelf)
            Shield.SetActive(false);
        else
            Shield.SetActive(true);
    }

    void ShotgunShot()
    {
        List<GameObject> ShotgunPellets = new List<GameObject>();
        Vector2 direction = (Player.transform.position - BulletSpawner.position).normalized;

        for (int i = 0; i < NumOfShots; i++)
        {
            ShotgunPellets.Add(Instantiate(Bullet, (direction * 1.5f) + new Vector2(transform.position.x, transform.position.y + Random.Range(-2,2)), new Quaternion()));
        }
    }

    void Stomp()
    {
        //Jump up and slam down in a part of the arena, flipping to the player
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PC.RemoveHealth(SlamDamage);
            PC.GettingHit();

            Vector2 movePlayerUp = new Vector2(Player.transform.position.x, Player.transform.position.y + 2);
            Player.GetComponent<Rigidbody2D>().AddForce((movePlayerUp - new Vector2(transform.position.x, transform.position.y)).normalized * KnockbackStrength, ForceMode2D.Impulse);
        }
    }

    void Rush()
    {

    }

    void CorruptionState()
    {
        //Will be immune from all damage except from Hakka
    }

}
