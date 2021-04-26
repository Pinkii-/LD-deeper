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
    public LevelsController levelcontroller;
    public Slider slider;
    public SpriteRenderer[] sprites;
    public float decreaseSpeed = 1f;
    public int secondsDead = 2;

    private Animator animator;
    bool healthdrop;



    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (levelcontroller == null) 
        {
            levelcontroller = GameObject.Find("Levels").GetComponent<LevelsController>();
        }
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

        slider.value = currentHealth / maxHealth;
        UpdateCharacterLight(currentHealth / maxHealth);


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
        CurrentHealth -= decreaseSpeed;
        healthdrop = false;
    }

    //The moment the player enters in contact with the Checkpoint all values are restored.
    public IEnumerator Die() 
    {
        isDead = true;
        animator.SetBool("isDead", true);
        this.GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(secondsDead);
        transform.position = lastCheckpoint.transform.position;
        levelcontroller.RestoreLevel();
        this.GetComponent<PlayerController>().enabled = true;
        animator.SetBool("isDead", false);
        Debug.Log("You died & rebirth");
    }



    private void UpdateCharacterLight(float value) 
    {
        //Debug.Log( "Update?" + value);
        for (int i = 0;  i < sprites.Length; i++) {
            sprites[i].color= new Color (sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, value);

        }
        
        //Color temp = gameObject.transform.Find("arm_light").GetComponent<SpriteRenderer>().color;
        //temp.a = 0f;
    }

}
