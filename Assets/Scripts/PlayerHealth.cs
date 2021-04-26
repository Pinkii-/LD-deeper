using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
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
        if (slider == null)
        {
            slider = GameObject.Find("Canvas").GetComponentInChildren<Slider>();
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
            Die();
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

    public void Die(bool inmediateRespawn = false)
    {
        StartCoroutine(DeferredDie(inmediateRespawn));
    }

    //The moment the player enters in contact with the Checkpoint all values are restored.
    public IEnumerator DeferredDie(bool inmediateRespawn = false) 
    {
        isDead = true;
        animator.SetBool("isDead", true);
        this.GetComponent<PlayerController>().enabled = false;

        if (!inmediateRespawn)
        {
            yield return new WaitForSeconds(secondsDead);
        }

        transform.position = lastCheckpoint.transform.position + new Vector3(1, 0 ,0);
        levelcontroller.RestoreLevel();
        this.GetComponent<PlayerController>().enabled = true;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.SetBool("isDead", false);
        animator.SetBool("isMoving", false);
    }
    
    private void UpdateCharacterLight(float value) 
    {
        //Debug.Log( "Update?" + value);
        /*for (int i = 0;  i < sprites.Length; i++) {
            sprites[i].color= new Color (sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, value);

        }*/
        
        //Color temp = gameObject.transform.Find("arm_light").GetComponent<SpriteRenderer>().color;
        //temp.a = 0f;

        var light = transform.GetComponentInChildren<Light2D>();
        light.pointLightOuterRadius = Mathf.Lerp(2.0f, 4.66f,value);
        light.color = Color.Lerp(new Color(1.0f, 0.1f, 0.1f), new Color(1, 0.9f, 0.55f), value);
    }
}
