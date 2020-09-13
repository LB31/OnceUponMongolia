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
    public SnapTurnProvider stp;

    public InputDevice LeftCon;

    private void Awake() {
        if (Instance)
            return;
        Instance = this;
    }

    public void ChangeHeadsetDependencies()
    {
        stp = FindObjectOfType<SnapTurnProvider>();
        if (OculusInUse)
            stp.turnUsage = SnapTurnProvider.InputAxes.Primary2DAxis;
        else
            stp.turnUsage = SnapTurnProvider.InputAxes.Secondary2DAxis;
    }
}
