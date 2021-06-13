using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Cinemachine.CinemachineVirtualCamera vCamera;

    public float StartFieldOfView;
    public float MovementIncrementForStart;
    public float WaitToTransition;

    public delegate void CameraAtStart();

    public static event CameraAtStart CameraInStartPosition;

    private float initOrthoSize;

    // Start is called before the first frame update
    void Start()
    {
        vCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        initOrthoSize = vCamera.m_Lens.OrthographicSize;
        GameManager.OnGameStart += GoToStartView;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GoToStartView()
    {
        StartCoroutine(MoveCameraSmoothlyToStartView());
    }

    public IEnumerator MoveCameraSmoothlyToStartView()
    {
        var cameraMoveIncrement = (StartFieldOfView - initOrthoSize) / MovementIncrementForStart;

        yield return new WaitForSeconds(.8f);


        while(vCamera.m_Lens.OrthographicSize< StartFieldOfView)
        {
            vCamera.m_Lens.OrthographicSize += cameraMoveIncrement;

            yield return new WaitForSeconds(.1f);
        }

        if(CameraInStartPosition != null)
            CameraInStartPosition();


    }
}
