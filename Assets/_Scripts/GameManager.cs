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
    public SnapTurnProvider SnapTurnProvider;

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
        SnapTurnProvider = FindObjectOfType<SnapTurnProvider>();
        if (OculusInUse)
            SnapTurnProvider.turnUsage = SnapTurnProvider.InputAxes.Primary2DAxis;
        else
            SnapTurnProvider.turnUsage = SnapTurnProvider.InputAxes.Secondary2DAxis;
    }
}
