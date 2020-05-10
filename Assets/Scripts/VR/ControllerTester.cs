using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerTester : MonoBehaviour
{

    public SteamVR_Action_Boolean BookMenu; 
    public SteamVR_Input_Sources handType;

    private void Start() {
        BookMenu.AddOnStateDownListener(TriggerDown, handType);
    }

    private void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        print("Pushed left trigger");
    }
}
