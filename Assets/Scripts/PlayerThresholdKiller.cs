using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThresholdKiller : MonoBehaviour
{
    public float fallSecondsToDie = 2;
    
    private float _timeFalling;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _timeFalling = 0;
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (!GetComponent<PlayerController>().IsGrounded)
        {
            _timeFalling += Time.deltaTime;

            if (_playerHealth && _timeFalling >= fallSecondsToDie)
            {
                if (!_playerHealth.isDead)
                {
                    _playerHealth.Die(true);
                }

                _timeFalling = 0;
            }
        }
        else
        {
            _timeFalling = 0;
        }
    }
}
