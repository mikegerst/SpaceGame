using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    
    [SerializeField] private GameObject MainBackground;
    [SerializeField] private GameObject[] Backgrounds;
    [SerializeField] private float MoveSpeed;

    private GameObject oldBackground;
    private Vector3 CreatePosition = new Vector3(0.17f, 29f, .39f);
    Vector3 screenBound;

    private GameObject currentBackground;
    private GameObject lastBackground;
    private bool destroyedLast = false;

    private void Start()
    {

        currentBackground = MainBackground;
    }

    private void Update()
    {
       // -10.94

        if(currentBackground.transform.position.y > -10.94f)
        {
            MoveBackground(currentBackground);
        }
        else
        {
            destroyedLast = false;
            lastBackground = currentBackground;
            currentBackground = Instantiate(currentBackground, CreatePosition, new Quaternion());
        }

        if(lastBackground != null && !destroyedLast)
        {
            if(lastBackground.transform.position.y < -30.0f)
            {
                destroyedLast = true;
                Destroy(lastBackground);

            }
            else
            {
                MoveBackground(lastBackground);
            }
        }

        
    }


    private void MoveBackground(GameObject background)
    {
        background.transform.position += Vector3.down * MoveSpeed * Time.deltaTime;
    }

}
