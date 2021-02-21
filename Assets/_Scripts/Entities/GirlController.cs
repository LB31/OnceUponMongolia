using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GirlController : EntityController
{


    private Vector2 inputAxis;

    private float turnSmoothVelocity;
    private PositionChanger positionChanger;
    public bool speakingWithVillager;

    private void OnEnable()
    {
        XRControls.Instance.ControllerEventTrigger -= Interact;
        XRControls.Instance.ControllerEventTrigger += Interact;
    }

    private void OnDisable()
    {
        XRControls.Instance.RemoveTriggerEvents();
    }

    public override void Start()
    {
        Character = GameManager.Instance.Vero;

        base.Start();

        positionChanger = GetComponent<PositionChanger>();
    }

    void Update()
    {
        GameManager.Instance.LeftCon.TryGetFeatureValue(GameManager.Instance.Axis2D, out inputAxis);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Gravity 
        if (characterController.isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        characterController.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

        if (speakingWithVillager) return;

        // Controlling
        if (inputAxis.magnitude > 0.2f)
        {
            moving = true;
            velocity = inputAxis.magnitude;

            Vector3 direction = new Vector3(inputAxis.x, 0, inputAxis.y).normalized;

            // rotate
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(Character.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
            Character.rotation = Quaternion.Euler(0f, angle, 0f);
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


       
    }

    private void LateUpdate()
    {
        // Move player behind girl
        transform.position = Character.position + positionChanger.CurrentVeroPosition.localPosition;
    }

    public void Interact()
    {
        if (GameManager.Instance.NearestVillager != null && 
            Vector3.Distance(GameManager.Instance.Vero.position, GameManager.Instance.NearestVillager.transform.position) < 4)
        {
            GameManager.Instance.NearestVillager.GetComponent<PlayMakerFSM>().SendEvent("StartDialog");
            speakingWithVillager = true;
        }
    }

    // Currently called by FSM event: Deactivate Dialog Box
    public void InteractionFinished()
    {
        speakingWithVillager = false;
        GameManager.Instance.NearestVillager = null;
    }


}
