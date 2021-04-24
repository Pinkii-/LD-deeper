using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public bool isSafe = true;
    bool healthdrop;

    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxHealth;
        healthdrop = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If in checkpoint life is restored, otherwise it goes down.
        if (isSafe)
        {
            CurrentHealth = maxHealth;
        }
        else if (!healthdrop)
        {
            healthdrop = true;
            StartCoroutine(ReduceHealth());
        }
        
    }



    // Timer reducing healthbar every X seconds.
    IEnumerator ReduceHealth() 
    {
        yield return new WaitForSeconds(1);
        CurrentHealth -= 1f;
        healthdrop = false;
    }
}
