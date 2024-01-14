using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//  Summary
//  This script is moving and rotating the camera
//  Summary
public class CameraActions : MonoBehaviour
{
    public static CameraActions Instance;
    [SerializeField] private List<CinemachineVirtualCamera> _cameras;

    [SerializeField] private float lerpSpeed;
    [SerializeField] private float lerpVelocity;
    [SerializeField] private float smoothTime;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;

    private CinemachineVirtualCamera _currentCamera;
    private int _currentCameraIndex;

    private float zoomValue;
    private float scrollValue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _currentCamera = _cameras[0];
        _currentCameraIndex = 0;

        zoomValue = _currentCamera.m_Lens.OrthographicSize;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        PlayerInputManager.MouseScroll += SetscrollValueValue;
        PlayerInputManager.NextCamera += SetNextCamera;
        PlayerInputManager.PreviousCamera += SetPreviousCamera;
    }

    private void OnDisable()
    {
        PlayerInputManager.MouseScroll -= SetscrollValueValue;
        PlayerInputManager.NextCamera -= SetNextCamera;
        PlayerInputManager.PreviousCamera -= SetPreviousCamera;
    }

    private void LateUpdate()
    {
        Zoom();
    }

    private void SetPreviousCamera()
    {
        ChangeCameraIndexValue(1);
        _cameras[_currentCameraIndex].m_Lens.OrthographicSize = _currentCamera.m_Lens.OrthographicSize;
        _currentCamera.m_Priority = 0;
        _currentCamera = _cameras[_currentCameraIndex];
        _currentCamera.m_Priority = 10;
    }

    private void SetNextCamera()
    {
        ChangeCameraIndexValue(-1);
        _cameras[_currentCameraIndex].m_Lens.OrthographicSize = _currentCamera.m_Lens.OrthographicSize;
        _currentCamera.m_Priority = 0;
        _currentCamera = _cameras[_currentCameraIndex];
        _currentCamera.m_Priority = 10;
    }

    private void ChangeCameraIndexValue(int value)
    {
        _currentCameraIndex += value;

        if (_currentCameraIndex > _cameras.Count - 1)
        {
            _currentCameraIndex = 0;
        }

        if (_currentCameraIndex < 0)
        {
            _currentCameraIndex = _cameras.Count - 1;
        }
    }

    private void SetscrollValueValue(float value)
    {
        scrollValue = value;
    }

    //  TODO: smooth zoom
    private void Zoom()
    {
        zoomValue -= Mathf.Clamp(scrollValue, -1f, 1f) * lerpSpeed;
        zoomValue = Mathf.Clamp(zoomValue, minDistance, maxDistance);

        _currentCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(_currentCamera.m_Lens.OrthographicSize, zoomValue, ref lerpVelocity, smoothTime);

        if (_currentCamera.m_Lens.OrthographicSize >= maxDistance)
        {
            _currentCamera.m_Lens.OrthographicSize = maxDistance;
        }
        if (_currentCamera.m_Lens.OrthographicSize <= minDistance)
        {
            _currentCamera.m_Lens.OrthographicSize = minDistance;
        }

        //camera.m_Lens.OrthographicSize = Mathf.Clamp(camera.m_Lens.OrthographicSize, minDistance, maxDistance);
    }

    public void ImpactShake(float duration, float strength)
    {
        transform.DOComplete();
        transform.DOShakePosition(duration, strength);
        transform.DOShakeRotation(duration, strength);
    }

    public void ImpactShake(float pDuration, float pStrength, float rDuration, float rStrength)
    {
        transform.DOComplete();
        transform.DOShakePosition(pDuration, pStrength);
        transform.DOShakeRotation(rDuration, rStrength);
    }
}
