using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    // Start is called before the first frame update
    [FMODUnity.EventRef] string fuelSound = "event:/Fuel";

    [SerializeField] private float distanceToAttract = 1.5f;
    [SerializeField] private float attarctIncrement = .001f;

    [SerializeField] private SpriteMask flare;
    [SerializeField] private SpriteMask burst;
    [SerializeField] private SpriteMask smoke;

    [SerializeField] private float distanceFromPlayerToPlaySound = 8f;

    public delegate void FuelCollected();
    public static event FuelCollected collectFuel;

    private bool moveSmokeAlphaUp = true;
    private bool movingTowardsPlayer = false;


    // Update is called once per frame
    void Update()
    {
        MoveMasks();

        if (CheckIfPlayerIsNear() && !movingTowardsPlayer)
        {
            MoveTowardsPlayer();
            movingTowardsPlayer = true;
            
            

        }
            
            
    }

    private void MoveMasks()
    {
        float smokeAlphaCutoffChange = 0.0001f;
        flare.transform.Rotate(Vector3.forward / 5);
        burst.transform.Rotate(Vector3.back / 3);
        smoke.transform.Rotate(Vector3.back / 8);

        if (moveSmokeAlphaUp)
        {
            smoke.alphaCutoff += smokeAlphaCutoffChange;

            if (smoke.alphaCutoff >= 0.25f)
                moveSmokeAlphaUp = false;
        }
        else
        {
            smoke.alphaCutoff -= smokeAlphaCutoffChange;

            if (smoke.alphaCutoff <= 0.16f)
                moveSmokeAlphaUp = true;
        }
    }

    private bool CheckIfPlayerIsNear()
    {
        return Vector3.Distance(transform.position, GameManager.Instance.GetPlayerPosition()) <= distanceToAttract;
    }

    private void MoveTowardsPlayer() {

        
        StartCoroutine("PlayerAttract");
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("UFO"))
        {
            
            transform.Rotate(GameManager.Instance.GetPlayerPosition(), 5f);
            collectFuel?.Invoke();
            Destroy(this.gameObject);

        }
    }

    IEnumerator PlayerAttract()
    {
        Debug.Log("Player Attract");
        float attractIncrease = .001f;
        bool soundPlayed = false;
        
        

        for (; ; )
        {
            Vector2 player = GameManager.Instance.GetPlayerPosition();
            if (Vector2.Distance(player,(Vector2)this.transform.position) <= distanceFromPlayerToPlaySound
                && !soundPlayed)
            {
                FMODUnity.RuntimeManager.PlayOneShot(fuelSound);
                soundPlayed = true;
            }
                

            transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, player.x, attarctIncrement),
                                             Mathf.SmoothStep(transform.position.y, player.y, attarctIncrement),
                                             -5);
            attarctIncrement += attractIncrease;

            attractIncrease *= 1.01f;
            
            yield return new WaitForEndOfFrame();
        }
    }
}
