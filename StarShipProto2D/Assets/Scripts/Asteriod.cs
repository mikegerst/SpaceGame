using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int laserHitToDestory = 3;
    [SerializeField] private float spinMax = 2.0f;
    [SerializeField] private float spinMin = 0.2f;
    [SerializeField] private ParticleSystem explosion;
    private int laserHitCount = 0;

    void Start()
    {
        int rand = Random.Range(0, 2);
        GetComponent<Rigidbody2D>().AddTorque(((rand == 1)?
                                                Random.Range(spinMin,spinMax + 1):
                                                -Random.Range(spinMin, spinMax + 1))
                                                * 10);
    }

    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.name.Contains("Laser"))
        {
            laserHitCount++;
            if (laserHitCount == laserHitToDestory)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
                
        }
    }
}
