using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float horizontalResolution = 1920;

    public float panSpeed = 5f;
    
    private Vector3 _targetPos;

    private void Start()
    {
        ScrollTo(0);
    }

    public void ScrollTo(float y)
    {
        _targetPos = new Vector3(transform.position.x, y, transform.position.z);
    }
    
    private void OnGUI()
    {
        float aspectRatio = Screen.width / Screen.height;
        Camera.main.orthographicSize = horizontalResolution / aspectRatio / 200;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPos, panSpeed * Time.deltaTime);
    }
}
