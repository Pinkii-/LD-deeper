using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatformBehavior : MonoBehaviour
{
    public int breakingTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        DeactivatePlatform();
    }


    IEnumerator DeactivatePlatform()
    {
        yield return new WaitForSeconds(breakingTime);
        gameObject.SetActive(false);
    }


}
