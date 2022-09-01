using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] float distanceTrigger = 10.0f;
    [SerializeField] float shootTrigger = 3.0f;

    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float MAX_VELOCITY = 10f;

    [SerializeField] int health;

    [FMODUnity.EventRef] string laserSound = "event:/Laser";
    [FMODUnity.EventRef] string shipHitSound = "event:/ShipHit";
    [FMODUnity.EventRef] string shipDestroyedSound = "event:/ShipExplosion";

    public GameObject laserPrefab;
    public GameObject shipExplosion;
    public Transform shootSpot;
    public float laserSpeed;
    public float ShotTimer = 1.0f;

    public int angleOffset = -90;


    Rigidbody2D rBody;

    float distanceToPlayer;

    float shotTimer = 0f;

    bool playerDead = false;
    bool alertFight = false;

    public static Action enemyInView;
    public static Action enemyDied;

    public delegate void InRange();
    public static event InRange enemyInSight;

    public UnityEvent moveCamera;

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        
        
    }

    void Update()
    {
       
        distanceToPlayer = Vector2.Distance(transform.position, GameManager.Instance.GetPlayerPosition());
        if (PlayerInRange())
        {
            if (!alertFight)
            {
                enemyInView?.Invoke();
                alertFight = true;
            }
            if (PlayerInShootingRange() && !playerDead)
            {
                
                InitiateFight();
            }
            else
            {

                MoveTowardsPlayer();
            }
        }
    }


    private void InitiateFight()
    {
        //StopMoving();
        LookAtPlayer();
        if (shotTimer <= 0f)
        {
            Shoot();
            shotTimer = ShotTimer;
        } 
        else
        {
            shotTimer -= Time.deltaTime;
        }
    }
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Laser"))
        {
            if (health > 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot(shipHitSound);
                health -= 1;
            }
            else
            {
                DestroyEnemyShip();
            }
                
        }
    }

    private void DestroyEnemyShip()
    {
        enemyDied?.Invoke();
        FMODUnity.RuntimeManager.PlayOneShot(shipDestroyedSound);
        Destroy(this.gameObject);
    }

    private void Shoot()
    {
        GameObject laser = Instantiate(laserPrefab, shootSpot.position, shootSpot.rotation);

        Rigidbody2D laser_Rbody = laser.GetComponent<Rigidbody2D>();

        laser_Rbody.AddForce(shootSpot.up * laserSpeed, ForceMode2D.Impulse);

        FMODUnity.RuntimeManager.PlayOneShot(laserSound);


    }

    private void MoveTowardsPlayer()
    {
        LookAtPlayer();
        MoveShipForward();
    }

    private void StopMoving()
    {
        rBody.velocity = Vector2.zero;
        rBody.angularVelocity = 0f;
    }
    private void PlayerDied()
    {
        playerDead = true;
    }
    private bool PlayerInRange()
    {
        return ((distanceToPlayer <= distanceTrigger) ? true: false );
    }

    private bool PlayerInShootingRange()
    {
        enemyInSight?.Invoke();
        return ((distanceToPlayer <= shootTrigger) ? true : false);
    }

    private void MoveShipForward()
    {
        if(rBody.velocity.magnitude < MAX_VELOCITY)
            rBody.AddForce(transform.up * moveSpeed * 10);
    }

    private void LookAtPlayer()
    {
        var diff = (Vector2)transform.position - GameManager.Instance.GetPlayerPosition();

        var angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - angleOffset;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnEnable()
    {
        Starship.playerDied += PlayerDied;
    }

    private void OnDisbale()
    {
        Starship.playerDied -= PlayerDied;
    }

   
}
