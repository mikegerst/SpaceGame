using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{

    [SerializeField] private float moveSpeed;

    Vector2 mousePosition;
    Vector2 nextPosition;
    
    Rigidbody2D rBody2D;
    float angle;
    bool controlsOn = false;
    Vector2 screenBounds;

    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        CameraControl.CameraInStartPosition += SetControlsOn;
        CameraControl.CameraInStartPosition += GetScreenBoundary;

    }

    private void Update()
    {
        SetMousePosition();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(controlsOn)
            MovePositionToMousePosition();
    }

    void MovePositionToMousePosition()
    {
        
        var posx = Mathf.Clamp(nextPosition.x, (screenBounds.x *-1) + objectWidth, screenBounds.x - objectWidth);
        var posy = Mathf.Clamp(nextPosition.y, (screenBounds.y *-1) + objectHeight, screenBounds.y - objectHeight);
        rBody2D.MovePosition(new Vector2(posx,posy));
    }

    void SetMousePosition()
    {
        
        
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log($"Mouse to World Point: {mousePosition} Mouse INPUT: {Input.mousePosition}");


        nextPosition =  transform.position - Camera.main.WorldToScreenPoint(mousePosition);
        //Debug.Log($"Position - mouse: {nextPosition}");
        nextPosition = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
        //Debug.Log($"FinalLERP: {nextPosition}");
        


    }

    void SetMousePositionShipCenter()
    {
        
         mousePosition = Input.mousePosition;
         mousePosition = Camera.main.ScreenToViewportPoint(mousePosition);
         Debug.Log("2: " + Camera.main.ViewportToScreenPoint(mousePosition) + " " + mousePosition);



         nextPosition = transform.position - Camera.main.ScreenToViewportPoint(mousePosition * 100);
         nextPosition = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
        
    }

    void GetScreenBoundary()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }
    void SetControlsOn()
    {
        controlsOn = true;
    }

    void SetControlsOff()
    {
        controlsOn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChangeChord(collision);

        ChangeNote(collision);
    }

    private void ChangeChord(Collider2D collision)
    {
        if (collision.name.Contains("Left"))
        {
            FmodTestForInstrument.ChangeChord(1);
        }
        else if (collision.name.Contains("Middle"))
        {
            FmodTestForInstrument.ChangeChord(2);
        }
        else if (collision.name.Contains("Right"))
        {
            FmodTestForInstrument.ChangeChord(3);
        }
    }

    private void ChangeNote(Collider2D collision)
    {
        var number = 0;
        if (collision.name.Contains("1"))
        {
            number = 1;
        }
        else if ((collision.name.Contains("2")))
        {
            number = 2;
        }
        else if ((collision.name.Contains("3")))
        {
            number = 3;
        }
        else if (collision.name.Contains("4"))
        {
            number = 4;
        }
        else if (collision.name.Contains("5"))
        {
            number = 5;
        }

        FmodTestForInstrument.ChangeNote(number);
    }
}
