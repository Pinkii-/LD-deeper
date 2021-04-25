using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public float cameraBottomOffset = 5f;
    private float effectDuration = 5f;
    
    private SpriteRenderer _renderer;
    private bool _visible = false;
    private float _elapsedTime = 0;
    
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        SetOverlayAlpha(1);
    }

    public void Reveal()
    {
        _visible = true;
        _elapsedTime = 0;
    }

    public void Hide()
    {
        _visible = false;
        _elapsedTime = 0;
    }

    public void Update()
    {
        _elapsedTime += Time.deltaTime;

        float newAlpha = Mathf.Lerp(_visible ? 1 : 0, _visible ? 0 : 1, _elapsedTime / effectDuration);
        SetOverlayAlpha(newAlpha);
    }

    private void SetOverlayAlpha(float alpha)
    {
        if (_renderer)
        {
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, alpha);
        }
    }
}
