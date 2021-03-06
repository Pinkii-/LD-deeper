using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatformBehavior : MonoBehaviour
{
    public int breakingTime = 5;
    private bool startPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startPlaying) 
        {
            if (!gameObject.GetComponent<Animation>().isPlaying) 
            {
                gameObject.GetComponent<Animation>().Play();

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        StartCoroutine(DeactivatePlatform(col));
        AudioManager.audioManagerRef.PlaySound("platformBreak");
    }


    IEnumerator DeactivatePlatform(Collision2D col)
    {
        startPlaying = true;
        gameObject.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(breakingTime);
        startPlaying = false;
        gameObject.SetActive(false);
    }
}
