using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Managers;
using UnityEngine;

public class ZoomInForMusic : MonoBehaviour
{
    // Start is called before the first frame update

    private CinemachineVirtualCamera vmCam;
    void Start()
    {
        vmCam = GetComponent<CinemachineVirtualCamera>();
        PlayStateManager.SceneChangingToMusic += ZoomIn;
    }

    // Update is called once per frame
    private void ZoomIn()
    {
        StartCoroutine(Zoom());
    }

    private IEnumerator Zoom()
    {
        while (vmCam.m_Lens.OrthographicSize > 2.5)
        {
            vmCam.m_Lens.OrthographicSize -= .1f;
            yield return new WaitForEndOfFrame();
        }
    }
}
