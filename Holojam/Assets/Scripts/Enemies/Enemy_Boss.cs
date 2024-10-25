using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float Health;
    [SerializeField] float SlamDamage;
    [SerializeField] float RushDamage;

    [Space(5)]
    [Header("Timers")]
    [SerializeField] float ActionTimer;
    float ActionTimerRunning = 0;
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
    bool ActionRunning = false;

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
    [SerializeField] GameObject LeftWall;
    [SerializeField] GameObject RightWall;

    int RandomAction = 0;
    int LastAction = 0;
    float windup = 0;
    GameObject RushTarget;
    bool RushHit = false;

    // Start is called before the first frame update
    void Start()
    {
        PC = Player.GetComponent<PlayerController>();

        SR = gameObject.GetComponent<SpriteRenderer>();

        OGColor = SR.color;

        SetRushTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionTimerRunning > 0)
            ActionTimerRunning -= Time.deltaTime;

        if (ShieldTimerRunning > 0)
            ShieldTimerRunning -= Time.deltaTime;

        if (ShieldTimerRunning <= 0)
            ShieldSwitch(false);

        if (CorruptionDurationRunning > 0)
            CorruptionDurationRunning -= Time.deltaTime;

        if (ShotTimerRunning > 0)
            ShotTimerRunning -= Time.deltaTime;

        if (EnterCorruptionTimerRunning > 0)
            EnterCorruptionTimerRunning -= Time.deltaTime;

        if (!ActionRunning && ActionTimerRunning <= 0)
        {
            RandomAction = Random.Range(1, 5);
            //Making sure it doesn't repeat an Support action twice in a row
            while(RandomAction == LastAction && (LastAction == 4 || LastAction == 5))
            {
                RandomAction = Random.Range(1, 5);
            }
            ActionTimerRunning = ActionTimer;
        }

        switch(RandomAction)
        {
            case 1://Shoot
                LastAction = 1;
                print("Shooting");
                break;
            case 2://Rush
                print("Rushing");
                LastAction = 2;
                if (windup < 1)
                    ColorWindup(Color.green, windup += Time.deltaTime);
                else
                    Rush();
                break;
            case 3://Stomp
                print("Stomping");
                LastAction = 3;
                break;
            case 4://Shield
                print("Shield Time");
                if (ShieldTimerRunning <= 0 && Shield.activeSelf == false)
                {
                    LastAction = 4;
                    ShieldSwitch(true);
                    ShieldTimerRunning = ShieldTimer;
                }
                break;
            case 5://Corruption State
                print("Entering Corruption State");
                LastAction = 5;
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

    void SetRushTarget()
    {
        if (Vector2.Distance(LeftWall.transform.position, transform.position) < Vector2.Distance(RightWall.transform.position, transform.position))
            RushTarget = RightWall;
        else
            RushTarget = LeftWall;
    }


    //Move Set
    private void ShieldSwitch(bool mode)
    {
        Shield.SetActive(mode);
    }

    void MachinegunBurst()
    {
        if (!ActionRunning)
        {
            ActionRunning = true;
            List<GameObject> Bullets = new List<GameObject>();
            Vector2 direction = (Player.transform.position - BulletSpawner.position).normalized;

            for (int i = 0; i < NumOfShots; i++)
            {
                Bullets.Add(Instantiate(Bullet, (direction * 1.5f) + new Vector2(transform.position.x, transform.position.y + Random.Range(-2, 2)), new Quaternion()));
            }
        }
        else
        {
            //continue the action until done
        }
    }

    void Stomp()
    {
        //Jump up and slam down in a part of the arena, flipping to the player
        if(!ActionRunning)
        {

            ActionRunning = true;
        }
        else
        {
            //this is only done when the action is done
            ActionRunning = false;
        }
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
        if (!ActionRunning)
        {
            Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), true);
            SR.color = OGColor;
            ActionRunning = true;
        }
        else
        {
            float DistToPlayer = Vector2.Distance(Player.transform.position, transform.position);
            if ( DistToPlayer < 4 && !RushHit)
            {
                PC.RemoveHealth(RushDamage);
                RushHit = true;
            }
            if(Vector2.Distance(RushTarget.transform.position, transform.position) < 10)
            {
                Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), false);
                windup = 0;
                SetRushTarget();
                ActionRunning = false;
                RushHit = false;
            }
        }
    }

    void CorruptionState()
    {
        //Will be immune from all damage except from Hakka
        if (!ActionRunning)
        {
            ActionRunning = true;
        }
        else
        {
            ActionRunning = false;
        }
    }

    void ColorWindup(Color c, float windup)
    {
        SR.color = Color.Lerp(OGColor, c, windup);
    }

}
