using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;
using Util;

public class Starship : MonoBehaviour
{

    FMODUnity.StudioEventEmitter fmodEmitter;
    [FMODUnity.EventRef] string stealthSound = "event:/ShipStealth";
    [FMODUnity.EventRef] string laserSound = "event:/Laser";
    [FMODUnity.EventRef] string music = "event:/Music";
    [FMODUnity.EventRef] string shipHit = "event:/ShipHit";
    [FMODUnity.EventRef] string shipExplosion = "event:/ShipExplosion";
    [FMODUnity.EventRef] string deathSound = "event:/Death";

    FMOD.Studio.EventInstance musicEvent;
    Rigidbody2D rigidBody;
    Material material;
    Vector2 position;
    Vector2 mousePosition;
    Vector2 screenBounds;

    float height;
    float width;
    float angle;
    
    public float moveSpeed;
    public float maxSpeed;

    public float laserForce;
    public float laserCoolDown = .3f;
    [SerializeField] private float _laserCoolDown = 0f;
    [SerializeField] private Texture2D cursorTex;

    public GameObject laserPrefab;
    public Transform shootSpot;
    public Scene mainScene;
    

    public float TimerForInvisiblty = 2.0f;

    public float minShader = 0.4f;
    public float maxShader = 2.0f;

    [SerializeField] private int angleOffset = 0;
    [SerializeField] private float health = 10;
    [SerializeField] private float damageAtSpeed = 10f;
    public float enemyTest = 0f;


    public float musicMovementCountStart = 10.0f;
    public float musicMovementCountStop = 2.0f;

    bool traveling = false;
    bool shipNotMoving = true;
    private Camera camera1;
    public float zoomTimer = 2f;
    private float _zoomTimer;
    private float _zoomInTimer;

    public delegate void Traveling();
    public static event Traveling traversing;

    public delegate void NotMoving();
    public static event NotMoving stayingStill;

    public delegate void Damage();
    public static event Damage tookDamage;

    public delegate void Dead();
    public static event Dead playerDied;

    public Action<Vector2> Reposition;


    void Start()
    {
        camera1 = Camera.main;

        rigidBody = GetComponent<Rigidbody2D>();

        fmodEmitter = GetComponent<FMODUnity.StudioEventEmitter>();

        material = GetComponent<SpriteRenderer>().material;

        screenBounds = camera1.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        width = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;

        material.SetFloat("_Fade", maxShader);

        _zoomTimer = zoomTimer;
        _zoomInTimer = zoomTimer;

        
        
        Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);

    }


    private void FixedUpdate()
    {
        if (_laserCoolDown > 0)
            _laserCoolDown -= .01f;
                

            SetShipMoveParameters();
            ShootLaser();

            SetDirection();
            ShipControlCheck();
            CheckMovementForCamera();

            
            fmodEmitter.SetParameter("MoveTime", rigidBody.velocity.magnitude);
            
            


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Laser"))
        {
            if(health> 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot(shipHit);
                health -= 1;
                tookDamage?.Invoke();
            }
            else
            {
                fmodEmitter.Stop();
                FMODUnity.RuntimeManager.PlayOneShot(shipExplosion);
                Destroy(this.gameObject);

                FMODUnity.RuntimeManager.PlayOneShot(deathSound);
                playerDied?.Invoke();
                
            }

        }
        else
        {
            if (rigidBody.velocity.magnitude >= damageAtSpeed)
            {
                FMODUnity.RuntimeManager.PlayOneShot(shipHit);
                health--;
                tookDamage?.Invoke();
            }
                
                        
        }
    }

    private void CheckMovementForCamera()
    {
        float magnitude = rigidBody.velocity.magnitude;

        CheckIfMovingForCamera(magnitude);
        CheckIfNotMovingForCamera(magnitude);
    }

  
   private void CheckIfMovingForCamera(float magnitude)
    { 
        if (magnitude >= maxSpeed/2 && _zoomTimer > 0)
        {
            _zoomTimer -= Time.deltaTime;
        }

        if (_zoomTimer <= 0)
        {
            if (traveling) return;

            traversing?.Invoke();
            traveling = true;
            _zoomInTimer = zoomTimer;

        }
    }

    private void CheckIfNotMovingForCamera(float magnitude) {

        if (magnitude <= 1)
            _zoomInTimer -= Time.deltaTime;

        if (_zoomInTimer <= 0)
        {
            if (!traveling) return;

            stayingStill?.Invoke();
            traveling = false;
            _zoomTimer = zoomTimer;

        }
    }

    private void ShipControlCheck()
    {
        //Takes Vert and hort inputs to set force direction
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);



        if (Input.GetKey(KeyCode.W))
        {
            
            rigidBody.AddForce(transform.up * moveSpeed, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.S))
        {
            if(rigidBody.velocity.magnitude > 0)
                rigidBody.AddForce(rigidBody.velocity * -moveSpeed, ForceMode2D.Impulse);
            
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(material.GetFloat("_Fade") >= maxShader)
                StartCoroutine(GoStealth());
        }

        if (rigidBody.velocity.magnitude > maxSpeed)
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, maxSpeed);

        KeepShipInBounds();

        
    }

    private void KeepShipInBounds()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        bool moved = false;

        if(x >= 675)
        {
            transform.position = new Vector3(-760, y, -5);
            moved = true;
        }
        else if( x <= -775)
        {
            transform.position = new Vector3(660, y, -5);
            moved = true;
        }
        else if(y >= 600)
        {
            transform.position = new Vector3(x, -660, -5);
            moved = true;
        }
        else if(y <= -675)
        {
            Vector3 pre = transform.position;
            transform.position = new Vector3(x, 660, -5);
            moved = true;
        }
        if (moved)
            Reposition?.Invoke(position);
    }

    IEnumerator GoStealth()
    {
        float _fade = material.GetFloat("_Fade");
        float incrementSec = .1f;
        float countTime = .4f;
        float count = 0;

        float maxPhase1 = 1.99f;
        float minPhase1 = 1.8f;

        float maxPhase2 = .99f;
        float minPhase2 = .8f;

        PlayStealthSound();
        
        while(count < countTime)
        {
            
            material.SetFloat("_Fade", UnityEngine.Random.Range(minPhase1, maxPhase1));
            count += incrementSec;
            yield return new WaitForSeconds(incrementSec);
        }

        count = 0;

        material.SetFloat("_Fade", maxPhase2);

        _fade = maxPhase2 - incrementSec;

        while (material.GetFloat("_Fade")>minShader)
        {
            
            material.SetFloat("_Fade", _fade);
            _fade -= incrementSec/5;
            yield return new WaitForSeconds(incrementSec);
        }

        
        
        yield return new WaitForSeconds(TimerForInvisiblty);

        PlayStealthSound();
        while (count < countTime)
        {

            material.SetFloat("_Fade", UnityEngine.Random.Range(minPhase2, maxPhase2));
            count += incrementSec;
            yield return new WaitForSeconds(incrementSec);
        }

        count = 0;
        while (count < countTime)
        {

            material.SetFloat("_Fade", UnityEngine.Random.Range(minPhase1, maxPhase1));
            count += incrementSec;
            yield return new WaitForSeconds(incrementSec);
        }

        material.SetFloat("_Fade", maxShader);

    }

    private void PlayLaserSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(laserSound);
    }

    private void PlayStealthSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(stealthSound);
    }

    private void SetShipMoveParameters()
    {
        mousePosition = Input.mousePosition;
        mousePosition = camera1.ScreenToWorldPoint(mousePosition);


        
        position = transform.position - camera1.WorldToScreenPoint(mousePosition);
        position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);

        
        angle = (Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg) - 90;

        
    }

    private void ShootLaser()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(_laserCoolDown <= 0)
            {
                GameObject laser = Instantiate(laserPrefab, shootSpot.position, shootSpot.rotation);
                Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();

                laserRb.AddForce(shootSpot.up * (laserForce + rigidBody.velocity.magnitude), ForceMode2D.Impulse);

                _laserCoolDown = laserCoolDown;
                PlayLaserSound();
            }
            
        }
    }

    private void SetDirection()
    {
        var dir = Input.mousePosition - camera1.WorldToScreenPoint(transform.position);

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
