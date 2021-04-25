using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public ParticleSystem runParticles;
    public ParticleSystem jumpParticles;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool startedWalking = false;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            jumpParticles.Play();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        /*bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }*/
        //Transform character = transform.Find("character");
        Transform character = this.transform;

        if (move.x < 0f && character.transform.localScale.x > 0f)
        {
            character.transform.localScale = new Vector3(character.transform.localScale.x * -1f, character.transform.localScale.y, character.transform.localScale.z);
        }
        else if (move.x > 0f && character.transform.localScale.x < 0f) 
        {
            character.transform.localScale = new Vector3(character.transform.localScale.x * -1f, character.transform.localScale.y, character.transform.localScale.z);
        }

        //Animation values
        animator.SetBool("isMoving", move.x != 0);
        animator.SetBool("isJumping", velocity.y != 0);

        if (move.x == 0) 
        {
            startedWalking = true;
        }

        if (startedWalking && move.x != 0 && velocity.y == 0) 
        {
            runParticles.Play();
            startedWalking = false;
        }



        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}