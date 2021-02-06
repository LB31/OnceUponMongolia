﻿using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GirlController : MonoBehaviour
{
    public Transform Walkie;

    public float Speed = 1;
    public float TurnSmoothTime = 0.1f;
    public float AnimationTime = 5f;

    private Vector2 inputAxis;
    private CharacterController characterController;
    private float gravity = -9.81f;
    private float fallingSpeed;

    private float turnSmoothVelocity;

    // Animation
    private float velocity;
    private Animator animator;
    private int velocityHash;
    private int timeHash;
    private float animTime;
    private bool increasingTime = true;
    private bool moving;

    // Debug
    private Vector3 offset;
    private PositionChanger pc;

    void Start()
    {
        characterController = Walkie.GetComponent<CharacterController>();
        animator = Walkie.GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        timeHash = Animator.StringToHash("Time");


        //offset = new Vector3(Walkie.position.x, Walkie.position.y + 8.0f, Walkie.position.z + 7.0f);
        pc = GetComponent<PositionChanger>();
    }

    void Update()
    {
        GameManager.Instance.LeftCon.TryGetFeatureValue(GameManager.Instance.Axis2D, out inputAxis);
    }

    private void FixedUpdate()
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


        if (inputAxis.magnitude > 0.2f)
        {
            moving = true;
            velocity = inputAxis.magnitude;

            Vector3 direction = new Vector3(inputAxis.x, 0, inputAxis.y).normalized;

            // rotate
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(Walkie.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
            Walkie.rotation = Quaternion.Euler(0f, angle, 0f);
            // move
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * Speed * Time.deltaTime * inputAxis.magnitude);

        }
        else if (moving)
        {
            moving = false;
            animTime = 0;
            increasingTime = true;
            velocity = 0;
        }


        animator.SetFloat(velocityHash, velocity);
        animator.SetFloat(timeHash, animTime);


        // gravity 
        if (characterController.isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        characterController.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    // Debug
    private void LateUpdate()
    {
        //offset = Quaternion.AngleAxis(inputAxis.x, Vector3.up) * offset;
        Quaternion rotation = Quaternion.Euler(50, Walkie.rotation.y, 100);
        transform.position = Walkie.position + pc.CurrentVeroPosition.localPosition;
    }


}
