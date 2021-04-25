using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighOrbBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float healthIncrease = 10;  
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
            other.GetComponent<PlayerHealth>().currentHealth += healthIncrease;
            if (other.GetComponent<PlayerHealth>().currentHealth > 100)
                other.GetComponent<PlayerHealth>().currentHealth = other.GetComponent<PlayerHealth>().maxHealth;
            this.gameObject.SetActive(false);
        }

    }

}
