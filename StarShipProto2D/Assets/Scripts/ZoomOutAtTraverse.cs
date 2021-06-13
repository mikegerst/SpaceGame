using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomOutAtTraverse : MonoBehaviour
{

    CinemachineVirtualCamera cameraView;
    private float cameraSize = 21;

    public float maxCameraSize = 30;
    void Start()
    {
        cameraView = GetComponentInChildren<CinemachineVirtualCamera>();
        Starship.traversing += ZoomOut;
        Starship.stayingStill += ZoomIn;
        Enemy.enemyInSight += ZoomOut;
    }

    void ZoomOut()
    {
        StartCoroutine("ZoomOutEase");
    }

    void ZoomIn()
    {
        Debug.Log("ZoomIN");
        StartCoroutine("ZoomInEase");
    }


    
    IEnumerator ZoomOutEase()
    {
        while(cameraView.m_Lens.OrthographicSize < maxCameraSize)
        {
            cameraView.m_Lens.OrthographicSize = Mathf.SmoothStep(
                cameraView.m_Lens.OrthographicSize,
                cameraView.m_Lens.OrthographicSize + 1f,
                .2f);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("ending zoom out");
    }

    IEnumerator ZoomInEase()
    {
        while (cameraView.m_Lens.OrthographicSize > cameraSize)
        {

            cameraView.m_Lens.OrthographicSize = Mathf.SmoothStep(
                cameraView.m_Lens.OrthographicSize -1f,
                cameraView.m_Lens.OrthographicSize,
                .2f);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDestroy()
    {
        Starship.traversing -= ZoomOut;
        Starship.stayingStill -= ZoomIn;
        Enemy.enemyInSight -= ZoomOut;
    }

}
