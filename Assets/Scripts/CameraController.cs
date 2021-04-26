using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float horizontalResolution = 1920;

    public float panSpeed = 5f;
    public float zoomSpeed = 0.5f;
    
    private Vector3 _targetPos;
    private float _targetZoom;

    private void Start()
    {
        float aspectRatio = Screen.width / Screen.height;
        Camera.main.orthographicSize = horizontalResolution / aspectRatio / 200;

        _targetPos = transform.position;
        _targetZoom = Camera.main.orthographicSize;
        
        //ZoomTo(1);
        ScrollTo(0);
    }

    public void ScrollTo(float y)
    {
        _targetPos = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void ZoomTo(float amount)
    {
        _targetZoom = Camera.main.orthographicSize * (amount / (Camera.main.orthographicSize * 2f));
    }
    
    private void OnGUI()
    {
        //float aspectRatio = Screen.width / Screen.height;
        //Camera.main.orthographicSize = horizontalResolution / aspectRatio / 200;
    }

    private void LateUpdate()
    {
        if (!Mathf.Approximately(Camera.main.orthographicSize, _targetZoom))
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, _targetZoom, zoomSpeed * Time.deltaTime);
        }
        
        if (Vector3.Distance(transform.position, _targetPos) >= 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPos, panSpeed * Time.deltaTime);
        }
    }
}
