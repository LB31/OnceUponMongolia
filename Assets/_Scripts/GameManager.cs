using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : Singleton<GameManager>
{

    public bool OculusInUse;
    public DeviceBasedSnapTurnProvider SnapTurnProvider;

    public InputDevice LeftCon;
    public InputDevice RightCon;

    public XRInput XRInputLeft;
    public XRInput XRInputRight;

    public InputFeatureUsage<Vector2> Axis2D;

    protected override void Awake() {
        base.Awake();

        GetXRInputs();
    }

    private void GetXRInputs()
    {
        XRInput[] inputs = FindObjectsOfType<XRInput>();
        int indexLeft = Array.IndexOf(inputs, inputs.First(con => con.controller.name.ToLower().Contains("left")));
        XRInputLeft = inputs[indexLeft];
        XRInputRight = inputs[indexLeft == 0 ? 1 : 0];
    }

    public void ChangeHeadsetDependencies()
    {
        SnapTurnProvider = FindObjectOfType<DeviceBasedSnapTurnProvider>();
        if (OculusInUse)
            SnapTurnProvider.turnUsage = DeviceBasedSnapTurnProvider.InputAxes.Primary2DAxis;
        else
            SnapTurnProvider.turnUsage = DeviceBasedSnapTurnProvider.InputAxes.Secondary2DAxis;
    }
}
