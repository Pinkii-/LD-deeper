using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public GameObject platformA;
    public GameObject platformB;
    public bool towardsB;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        towardsB = true;
        startPosition = platformA.transform.position;
        endPosition = platformB.transform.position;
        StartCoroutine(Vector3LerpCoroutine(gameObject, endPosition, moveSpeed));

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (transform.position == endPosition)
        {
            StartCoroutine(Vector3LerpCoroutine(gameObject, startPosition, moveSpeed));
            towardsB = false;
        }
        else if (transform.position == startPosition)
        {
            towardsB = true;
            StartCoroutine(Vector3LerpCoroutine(gameObject, endPosition, moveSpeed));
        }
    }


    //GameObject moves towards Target and stops when gets to target.
    IEnumerator Vector3LerpCoroutine(GameObject obj, Vector3 target, float speed)
    {
        Vector3 startPosition = obj.transform.position;
        float time = 0f;

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.Lerp(startPosition, target, (time / Vector3.Distance(startPosition, target)) * speed);
            // this.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(startPosition, target, (time / Vector2.Distance(startPosition, target)) * speed));
            time += Time.deltaTime;
            yield return null;
        }
    }

    //Parent player so it moves along with the platform
    void OnCollisionEnter2D(Collision2D col)
    {
        col.collider.transform.SetParent(transform);
        //col.gameObject.transform.SetParent(gameObject.transform);
        //col.gameObject.transform.parent = gameObject.transform;
        //col.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }


    void OnCollisionExit2D(Collision2D col)
    {
        col.transform.parent = null;
    }


}
