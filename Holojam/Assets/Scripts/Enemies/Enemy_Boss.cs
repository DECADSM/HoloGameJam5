using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float Health;
    [SerializeField] float SlamDamage;
    [SerializeField] float RushDamage;
    [SerializeField] float RushForce;
    [SerializeField] float DMGMultiplier;
    [SerializeField] float KnockbackStrength;
    [SerializeField] float HeightAbovePlayer;
    [SerializeField] float JumpForce;
    [SerializeField] float StompForce;

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
    [SerializeField] float StompWait;
    float StompWaitRunning = 0;

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

    List<KeyValuePair<GameObject, Vector2>> Bullets;

    // Start is called before the first frame update
    void Start()
    {
        PC = Player.GetComponent<PlayerController>();

        SR = gameObject.GetComponent<SpriteRenderer>();

        OGColor = SR.color;
        Bullets = new List<KeyValuePair<GameObject, Vector2>>();

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

        if (StompWaitRunning > 0)
            StompWaitRunning -= Time.deltaTime;

        float DistToPlayer = Vector2.Distance(Player.transform.position, transform.position);
        if (DistToPlayer < 4 && !RushHit)
        {
            PC.RemoveHealth(RushDamage);
            RushHit = true;
        }

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

        switch (RandomAction)
        {
            case 1://Shoot
                LastAction = 1;
                print("Shooting");
                MachinegunBurst();
                break;
            case 2://Rush
                print("Rushing");
                Rush();
                LastAction = 2;
                break;
            case 3://Stomp
                print("Stomping");
                Stomp();
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
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawRay(transform.position, new Vector2(Player.transform.position.x - transform.position.x, HeightAbovePlayer + 20 - transform.position.y).normalized * 5);
    }

    //Move Set
    private void ShieldSwitch(bool mode)
    {
        Shield.SetActive(mode);
    }

    void MachinegunBurst()
    {
        if (windup < 1)
            ColorWindup(Color.blue, windup += Time.deltaTime * .5f);

        if (!ActionRunning && windup >= 1)
        {
            SR.color = OGColor;
            float offset = -4;
            for (int i = 0; i < NumOfShots; i++)
            {
                Vector2 direction = new Vector2(Player.transform.position.x + offset - BulletSpawner.position.x, Player.transform.position.y + offset - BulletSpawner.position.y).normalized;
                if(offset <= 8)
                    offset += 3;
                GameObject b = (Instantiate(Bullet, (direction) + new Vector2(BulletSpawner.position.x, BulletSpawner.position.y), new Quaternion()));
                if (b != null)
                    print("bullet spawned");
                b.GetComponent<Bullet>().parent = gameObject;
                Bullets.Add(new KeyValuePair<GameObject, Vector2>(b, direction));
            }
            Bullets[0].Key.GetComponent<Rigidbody2D>().velocity = Bullets[0].Value * Bullets[0].Key.GetComponent<Bullet>().speed;
            Bullets.RemoveAt(0);
            ShotTimerRunning = ShotTimer;
            ActionRunning = true;
        }
        else if(ActionRunning && windup >= 1)
        {
            if(ShotTimerRunning <= 0)
            {
                Bullets[0].Key.GetComponent<Rigidbody2D>().velocity = Bullets[0].Value * Bullets[0].Key.GetComponent<Bullet>().speed;
                Bullets.RemoveAt(0);
                ShotTimerRunning = ShotTimer;
            }
            if (Bullets.Count <= 0)
            {
                windup = 0;
                RandomAction = 0;
                ActionRunning = false;
            }
            //continue the action until done
        }
    }

    void Stomp()
    {
        if (windup < 1)
            ColorWindup(Color.magenta, windup += Time.deltaTime * .5f);
        Vector2 direction = new Vector2(Player.transform.position.x - transform.position.x, HeightAbovePlayer - transform.position.y).normalized;
        //Jump up and slam down in a part of the arena, flipping to the player
        if(!ActionRunning && windup >= 1)
        {
            SR.color = OGColor;
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            ActionRunning = true;
        }
        else if(ActionRunning && windup >= 1)
        {
            if(transform.position.y >= HeightAbovePlayer && StompWaitRunning <= 0 && !StompCollider.gameObject.activeSelf)
            {
                transform.position = new Vector2(transform.position.x, HeightAbovePlayer);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                StompCollider.gameObject.SetActive(true);
                StompWaitRunning = StompWait;
            }
            else if (StompWaitRunning <= 0 && transform.position.y == HeightAbovePlayer)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * StompForce, ForceMode2D.Impulse);
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                windup = 0;
                RandomAction = 0;
                ActionRunning = false;
            }
            else if(transform.position.y == HeightAbovePlayer)
                transform.position = new Vector2(Player.transform.position.x, transform.position.y);
            //this is only done when the action is done
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") )
        {
            PC.RemoveHealth(SlamDamage);
            PC.GettingHit();

            int left = Random.Range(0, 2);
            if(left == 1)
                Player.GetComponent<Rigidbody2D>().AddForce(Vector2.left * KnockbackStrength, ForceMode2D.Impulse);
            else
                Player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * KnockbackStrength, ForceMode2D.Impulse);
        }

        StompCollider.gameObject.SetActive(false);
    }

    void Rush()
    {
        if(windup < 1)
            ColorWindup(Color.green, windup += Time.deltaTime * .5f);

        
        Vector2 direction = new Vector2(RushTarget.transform.position.x - transform.position.x, 0).normalized;

        if (!ActionRunning && windup >= 1)
        {
            Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), true);
            SR.color = OGColor;
            ActionRunning = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(direction * RushForce, ForceMode2D.Impulse);
        }
        else if(ActionRunning && windup >= 1)
        {
            
            if(Vector2.Distance(RushTarget.transform.position, transform.position) < 10)
            {
                Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), false);
                if(transform.rotation.y == 0)
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
                else
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                windup = 0;
                SetRushTarget();
                ActionRunning = false;
                RushHit = false;
                RandomAction = 0;
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
