using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame updateri
    Rigidbody2D rigidBody;
    Quaternion dir;
    [SerializeField]
    float TimeAlive = 2.0f;
    [SerializeField]
    GameObject explosion;
    Collider2D coll;

   

    private void Update()
    {
        if (TimeAlive > 0)
            TimeAlive -= Time.deltaTime;
        else
            Destroy(this.gameObject);

        coll = GetComponent<Collider2D>();

        Physics2D.IgnoreLayerCollision(9, 9);
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.name.Contains("Laser"))
        {
            Debug.Log(collision.gameObject.name);
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
            Destroy(gameObject);
        }
        
    }



}
