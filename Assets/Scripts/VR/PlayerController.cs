using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public float WalkingSpeed = 1;

    // Testing
    public Transform Walkie;

    private CharacterController characterController;

    void Start() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {

        if (input.axis.magnitude > 0.6f) {
            //Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
            //characterController.Move(WalkingSpeed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);
            
            Walkie.Rotate(Vector3.up * input.axis.x * 2);
        }

        float spinOffset = 0.8f;

        if(input.axis.y > spinOffset || input.axis.y < -spinOffset) {
            Walkie.Translate(transform.forward * input.axis.y / 10);
        }


    }
}
