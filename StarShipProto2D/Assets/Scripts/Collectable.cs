using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float speed = 1f;

    private void Update()
    {
        transform.position += Time.deltaTime * speed * Vector3.down;
        if(Mathf.Abs(transform.position.y) > 23)
        {
            Hide();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("UFO"))
        {
            Hide();
        }
    }

    private void PlayerTouchedTrigger()
    {
        Destroy(this.gameObject);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
