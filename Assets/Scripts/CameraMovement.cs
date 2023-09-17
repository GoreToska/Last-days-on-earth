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
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;

    private void Awake()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        Zoom();
    }

    //  TODO: smooth zoom
    private void Zoom()
    {
        camera.m_Lens.OrthographicSize = Mathf.Lerp(camera.m_Lens.OrthographicSize, Mathf.Clamp(camera.m_Lens.OrthographicSize - PlayerInputManager.Instance.Scroll / lerpSpeed,
            minDistance, maxDistance), lerpSpeed * Time.deltaTime);
    }
}
