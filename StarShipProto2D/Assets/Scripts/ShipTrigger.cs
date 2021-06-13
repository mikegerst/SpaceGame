using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShipTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public Light2D lightForTrigger;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "UFO")
        {
            lightForTrigger.intensity += .5f;
        }

        Debug.Log($"{collision.gameObject.name} Entered Collider");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == "UFO")
        {
            lightForTrigger.intensity -= .5f;
        }
    }
}
