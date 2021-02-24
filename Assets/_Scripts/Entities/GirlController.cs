using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GirlController : EntityController
{
    // Simulator
    public GameObject SimulatorObj;

    public float InteractionDistance = 4;

    private Vector2 inputAxis;
    private float turnSmoothVelocity;
    private PositionChanger positionChanger;
    // TODO private
    public bool speakingWithVillager;
    private Transform player;
    // Simulator
    private bool useSimulator;

    private void OnEnable()
    {
        XRControls.Instance.ControllerEventTrigger -= Interact;
        XRControls.Instance.ControllerEventTrigger += Interact;
        Simulator.ButtonEvent.RemoveAllListeners();
        Simulator.ButtonEvent.AddListener(Interact);
    }

    private void OnDisable()
    {
        XRControls.Instance.RemoveTriggerEvents();
    }

    public override void Start()
    {
        Character = GameManager.Instance.Vero;

        base.Start();

        positionChanger = FindObjectOfType<PositionChanger>();
        player = FindObjectOfType<XRRig>().transform;

        // Simulator
        if (SimulatorObj && SimulatorObj.activeSelf)
            useSimulator = true;
    }

    void Update()
    {
        GameManager.Instance.LeftCon.TryGetFeatureValue(GameManager.Instance.Axis2D, out inputAxis);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (speakingWithVillager || !characterController.enabled) return;

        // Gravity 
        if (characterController.isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        characterController.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

        // Controlling
        if (inputAxis.magnitude > 0.2f || Simulator.Movement.magnitude > 0.2f)
        {
            moving = true;

            Vector2 movement = useSimulator ? Simulator.Movement : inputAxis;

            velocity = movement.magnitude;

            Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

            //if (useSimulator)
            //{
            //    characterController.Move(Speed * Time.deltaTime * new Vector3(movement.x, 0f, movement.y));
            //    return;
            //}

            // rotate
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(Character.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
            Character.rotation = Quaternion.Euler(0f, angle, 0f);
            // move
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * Speed * Time.deltaTime * movement.magnitude);

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
        player.position = Character.position + positionChanger.CurrentVeroPosition.localPosition;
    }

    public void Interact()
    {
        GameManager gm = GameManager.Instance;

        if (gm.NearestVillager != null && Vector3.Distance(gm.Vero.position, gm.NearestVillager.transform.position) < InteractionDistance)
        {
            gm.NearestVillager.GetComponent<PlayMakerFSM>().SendEvent("StartDialog");
            speakingWithVillager = true;
            Debug.LogError("y u interact againo?!");
        }
    }

    // Currently called by FSM event: Deactivate Dialog Box
    public void InteractionFinished()
    {
        print("interaction finished");
        GameManager.Instance.NearestVillager = null;
        speakingWithVillager = false;
    }


}
