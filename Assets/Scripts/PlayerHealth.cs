using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public bool isSafe = true;
    public bool isDead = false;
    public GameObject lastCheckpoint;
    public Slider slider;

    private Animator animator;
    bool healthdrop;



    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        RestoreHealth();
    }

    // Update is called once per frame
    void Update()
    {
        //If in checkpoint life is restored, otherwise it goes down.
        if (isSafe)
        {
            RestoreHealth();
        }
        else if (!healthdrop)
        {
            healthdrop = true;
            StartCoroutine(ReduceHealth());
        }


        //If Health reaches 0 go back to the last checkpoint
        if (currentHealth <= 0) 
        {
            StartCoroutine( Die());
        }

        slider.value = currentHealth / 100f;

    }

    private void RestoreHealth() 
    {
        CurrentHealth = maxHealth;
        healthdrop = false;
        isDead = false;
    }

    // Timer reducing healthbar every X seconds.
    IEnumerator ReduceHealth() 
    {
        yield return new WaitForSeconds(1);
        CurrentHealth -= 1f;
        healthdrop = false;
    }

    //The moment the player enters in contact with the Checkpoint all values are restored.
    public IEnumerator Die() 
    {
        isDead = true;
        animator.SetBool("isDead", true);
        //TODO: Stop controller for 2 secs.
        this.GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(2);
        transform.position = lastCheckpoint.transform.position;
        this.GetComponent<PlayerController>().enabled = true;
        animator.SetBool("isDead", false);
        Debug.Log("You died & rebirth");
    }
}
