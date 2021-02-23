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
    public Transform Vero;
    public Transform Player;
    public VillagerController HailStone;

    public Transform SkyDome;
    public float SkySpeed = 0.1f;

    public VillagerController NearestVillager;

    public List<MarkerCollector> QuestMarkers;

    [HideInInspector] public bool OculusInUse;
    [HideInInspector] public DeviceBasedSnapTurnProvider SnapTurnProvider;
    [HideInInspector] public XRInput XRInputLeft;
    [HideInInspector] public XRInput XRInputRight;
    public InputDevice LeftCon;
    public InputDevice RightCon;
    public InputFeatureUsage<Vector2> Axis2D;

    protected override void Awake()
    {
        base.Awake();

        Player = FindObjectOfType<XRRig>().transform;

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

    private void Update()
    {
        if (SkyDome)
            SkyDome.Rotate(new Vector3(0, SkySpeed, 0));
    }
}

[Serializable]
public class MarkerCollector
{
    public string MarkerName;
    public Person MarkerOwner;
    public GameObject MarkerObject;
}
