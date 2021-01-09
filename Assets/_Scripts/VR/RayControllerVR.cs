using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayControllerVR : MonoBehaviour
{
    private XRController xrController;
    private XRRayInteractor rayInteractor;

    private void Awake()
    {
        xrController = GetComponent<XRController>();
        rayInteractor = GetComponent<XRRayInteractor>();

        Initialize();
    }

    private void Initialize()
    {
        xrController.moveObjectOut = InputHelpers.Button.SecondaryAxis2DUp;

        rayInteractor.maxRaycastDistance = 10;
        rayInteractor.lineType = XRRayInteractor.LineType.ProjectileCurve;
    }
}
