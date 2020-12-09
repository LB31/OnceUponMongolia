using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool OculusInUse;
    public DeviceBasedSnapTurnProvider SnapTurnProvider;

    public InputDevice LeftCon;
    public InputDevice RightCon;

    public InputFeatureUsage<Vector2> Axis2D;

    private void Awake() {
        if (Instance)
            return;
        Instance = this;
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
