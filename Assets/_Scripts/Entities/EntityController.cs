using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{

    public float Speed = 1;
    public float TurnSmoothTime = 0.1f;
    public float AnimationTime = 5f;

    protected Transform Character;
    protected CharacterController characterController;
    protected float gravity = -9.81f;
    protected float fallingSpeed;

    // Animation
    protected float velocity;
    protected Animator animator;
    protected int velocityHash;
    protected int timeHash;
    protected float animTime;
    protected bool increasingTime = true;
    protected bool moving;


    public virtual void Start()
    {
        characterController = Character.GetComponent<CharacterController>();
        animator = Character.GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        timeHash = Animator.StringToHash("Time");
    }

    public virtual void FixedUpdate()
    {
        if (increasingTime)
            animTime += Time.deltaTime;
        else
            animTime -= Time.deltaTime;

        if (animTime >= AnimationTime)
        {
            increasingTime = !increasingTime;
        }
        if (animTime <= 0)
        {
            increasingTime = !increasingTime;
        }

        // Hravity 
        if (characterController.isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        characterController.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

        // Animations
        animator.SetFloat(velocityHash, velocity);
        animator.SetFloat(timeHash, animTime);

    }
}
