using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GirlController : MonoBehaviour
{

    public float Speed = 1;
    public XRNode InputSource;
    public float AdditionalHeight = 0.2f;

    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController character;
    private InputDevice device;
    private float gravity = -9.81f;
    private float fallingSpeed;

    // Testing
    public Transform Walkie;


    void Start()
    {
        rig = GetComponent<XRRig>();
        character = Walkie.GetComponent<CharacterController>();
        device = InputDevices.GetDeviceAtXRNode(InputSource);
    }

    void Update()
    {
        device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        if (inputAxis.magnitude > 0.6f)
        {
            //Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
            //characterController.Move(WalkingSpeed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);

            Walkie.Rotate(Vector3.up * inputAxis.x * Speed);
        }

        float spinOffset = 0.8f;

        if (inputAxis.y > spinOffset || inputAxis.y < -spinOffset)
        {
            character.Move(Walkie.forward * inputAxis.y / Speed);
        }

        // gravity 
        if (character.isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }


}
