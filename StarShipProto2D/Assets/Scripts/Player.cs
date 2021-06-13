using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody body;
    [SerializeField]
    private float speed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            transform.position += transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.position -= transform.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position += transform.right * speed * Time.deltaTime;
    }
}
