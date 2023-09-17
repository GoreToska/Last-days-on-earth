using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Summary
//  This script is moving and rotating the camera
//  Summary
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMovement : MonoBehaviour
{
    private CinemachineVirtualCamera camera;

    [SerializeField] private float lerpSpeed;
    [SerializeField] private float lerpVelocity;
    [SerializeField] private float smoothTime;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;

    private float zoomValue;

    private void Awake()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        zoomValue = camera.m_Lens.OrthographicSize;
    }

    private void LateUpdate()
    {
        Zoom();
    }

    //  TODO: smooth zoom
    private void Zoom()
    {
        zoomValue -= Mathf.Clamp(PlayerInputManager.Instance.Scroll, -1f, 1f) * lerpSpeed;
        zoomValue = Mathf.Clamp(zoomValue, minDistance, maxDistance);

        camera.m_Lens.OrthographicSize = Mathf.SmoothDamp(camera.m_Lens.OrthographicSize, zoomValue, ref lerpVelocity, smoothTime);

        if (camera.m_Lens.OrthographicSize >= maxDistance)
        {
            camera.m_Lens.OrthographicSize = maxDistance;
        }
        if (camera.m_Lens.OrthographicSize <= minDistance)
        {
            camera.m_Lens.OrthographicSize = minDistance;
        }

        //camera.m_Lens.OrthographicSize = Mathf.Clamp(camera.m_Lens.OrthographicSize, minDistance, maxDistance);
    }
}
