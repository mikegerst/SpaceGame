using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousRotation : MonoBehaviour
{

    public float rotationSpeed;
    public bool rotate;
    public Vector3 rotationAngle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Rotate());

    }


    IEnumerator Rotate()
    {


        while (rotate)
        {
            
            transform.Rotate(rotationAngle * (rotationSpeed * Time.deltaTime), Space.Self);
            yield return new WaitForEndOfFrame();
        }


        
    }
}
