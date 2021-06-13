using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Dim : MonoBehaviour
{
    private Light2D light;


    void Start()
    {
        light = GetComponent<Light2D>();
        PlayStateManager.SceneChangingToMusic += DimToZero;
    }

    void DimToZero()
    {
        StartCoroutine(DimLight());
    }

    IEnumerator DimLight()
    {

        while (light.intensity > 0)
        {
            light.intensity -= .1f;
            yield return new WaitForSeconds(0.2f);
        }
        
    }
    
}
