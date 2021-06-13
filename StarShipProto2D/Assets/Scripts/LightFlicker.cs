using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public Light2D light2D;
    
    public float maxIntensity;
    

    public float incrementForFlicker;

    public float UpdatePerSec;

    float minIntensity;
    float initIntensity;
    float initInnerRadius;
    float initOuterRadius;

    bool increase = true;

    bool flicker = true;
    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponent<Light2D>();
        initIntensity = light2D.intensity;
        initInnerRadius = light2D.pointLightInnerRadius;
        initOuterRadius = light2D.pointLightOuterRadius;

        minIntensity = initIntensity;

        Debug.Log(minIntensity);
        StartCoroutine(Flicker());

        
    }

    

    IEnumerator Flicker()
    {
        

        while (flicker)
        {
            if (light2D.intensity >= maxIntensity)
            {
                increase = false;
            }

            if (light2D.intensity <= minIntensity)
            {
                increase = true;
            }


            if (increase)
            {
                light2D.intensity += incrementForFlicker;
                light2D.pointLightInnerRadius += incrementForFlicker;
                light2D.pointLightOuterRadius += incrementForFlicker;
            }
            else
            {
                light2D.intensity -= incrementForFlicker;
                light2D.pointLightInnerRadius -= incrementForFlicker;
                light2D.pointLightOuterRadius -= incrementForFlicker;
            }

            yield return new WaitForSeconds(UpdatePerSec);
        }
        
    }

}
