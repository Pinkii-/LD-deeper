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
        GoToLevel(0);
    }

    public void GoToLevel(int level)
    {
        float targetY = LevelsController.Instance.GetLevelY(level);
        _targetPos = new Vector3(transform.position.x, targetY, transform.position.z);
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
