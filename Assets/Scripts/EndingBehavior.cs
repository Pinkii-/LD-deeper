using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingBehavior : MonoBehaviour
{
    public GameObject bodylight;
    public GameObject headlight;
    public GameObject treelight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>() != null) 
        {
            other.GetComponent<PlayerHealth>().isSafe = true;
            other.GetComponent<PlayerController>().enabled = false;
            bodylight.SetActive(true);
            headlight.SetActive(true);
            treelight.SetActive(true);
            other.GetComponentInChildren<Animator>().SetBool("isDead", true);

        }

    }
}


