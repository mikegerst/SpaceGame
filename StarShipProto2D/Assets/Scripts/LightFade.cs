using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFade : MonoBehaviour
{
    public Vector3 reduce;
    public float maxSize;

    
    // Update is called once per frame


    void Update()
    {
        if (transform.localScale.x < 0 || transform.localScale.x > maxSize)
            Destroy(gameObject);
        else
            transform.localScale += reduce;

    }
}
