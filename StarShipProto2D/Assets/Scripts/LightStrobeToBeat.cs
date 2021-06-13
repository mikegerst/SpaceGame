using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightStrobeToBeat : MonoBehaviour
{
    Light2D light;

    float initIntensity;

    public float maxIntensity;
    public float fullyExpandedLightRadius;
    public enum StrobeType
    {
        Radius,
        Intensity,
        Both
        
    }

    public StrobeType strobeType;
    public bool changeColor;
    public bool matchColor;

    static Color matchedColor;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
        initIntensity = light.intensity;
        //CameraControl.CameraInStartPosition += LightAtStartSettings;
        FModManager.BeatChange += StartLightExpand;
        FModManager.BeatChange += SetMatchedColor;
    }

    // Update is called once per frame
    void Update()
    {
        ReduceStrobe();
        
    }


    public void ReduceStrobe()
    {
        if (strobeType.Equals(StrobeType.Radius))
        {
            if (light.pointLightOuterRadius > 0)
            {
                light.pointLightOuterRadius -= .25f;
            }
        }
        else if (strobeType.Equals(StrobeType.Intensity))
        {
            if (light.intensity > 0)
            {
                light.intensity -= .01f;
            }
        }
        else if (strobeType.Equals(StrobeType.Both))
        {
            if(light.intensity > 0 && light.pointLightOuterRadius > 0)
            {
                light.pointLightOuterRadius -= .25f;
                light.intensity -= .01f;
            }
            
        }
    }


    public void LightAtStartSettings()
    {
        light.intensity = maxIntensity;
    }

    public void StartLightExpand()
    {
        
        StartCoroutine(Strobe());
    }

    public void SetMatchedColor()
    {
        matchedColor = Random.ColorHSV(0f,1f,0f,1f,.5f,1f);
        matchedColor.a = 1;
    }



    IEnumerator Strobe()
    {
        if (strobeType.Equals(StrobeType.Radius))
            light.pointLightOuterRadius = fullyExpandedLightRadius;
        else if (strobeType.Equals(StrobeType.Intensity))
            light.intensity = maxIntensity;
        else if (strobeType.Equals(StrobeType.Both))
        {
            light.intensity = maxIntensity;
            light.pointLightOuterRadius = fullyExpandedLightRadius;
        }

        if (changeColor)
        {
            if (matchColor)
            {   
                light.color = matchedColor;
                
            }
            else
            {
                light.color = Random.ColorHSV(0f, 1f, 0f, 1f, .5f, 1f);
            }
        }
        
        
        yield return new WaitForSeconds(0.001f);
    }
}
