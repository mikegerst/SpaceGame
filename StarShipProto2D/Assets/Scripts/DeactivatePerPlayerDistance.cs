using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatePerPlayerDistance : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float maxDistance = 200f;

    private Coroutine checkDistance;
    private void OnEnable()
    {
        checkDistance = StartCoroutine(CheckDistanceAndDeactivate());
    }

    private void OnDisable()
    {
        if (checkDistance != null) 
            StopCoroutine(checkDistance);
    }

    IEnumerator CheckDistanceAndDeactivate()
    {

        while(Vector2.Distance(GameManager.Player.transform.position, transform.position) <= maxDistance)
            yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

}
