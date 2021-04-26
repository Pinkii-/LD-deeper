using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public GameObject platformA;
    public GameObject platformB;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 waypoint;
    protected Rigidbody2D rb2d;
    
    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPosition = platformA.transform.position;
        endPosition = platformB.transform.position;
        
        transform.position = startPosition;
        waypoint = endPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentPos = transform.position;
        
        if (Vector3.Distance(currentPos, waypoint) < 0.05f)
        {
            waypoint = waypoint == startPosition ? endPosition : startPosition;
        }
        
        Vector3 waypointDir = (waypoint - currentPos).normalized;
        rb2d.MovePosition(currentPos + waypointDir * (moveSpeed * Time.deltaTime));
    }

    //Parent player so it moves along with the platform
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.transform.parent = null;
        }
    }
}
