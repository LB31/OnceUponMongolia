using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class XRControls : Singleton<XRControls>
{
    public delegate void RightControllerButton(bool side);

    public delegate void ControllerTrigger();  
    public delegate void ControllerGrip();


    public event RightControllerButton ControllerEventButton; // Here A and B
    public event ControllerTrigger ControllerEventTrigger;
    public event ControllerGrip ControllerEventGrip;

    private XRBinding rightControllerA; // Right touch on WMR
    private XRBinding rightControllerB; // Left touch on WMR

    private XRBinding rightControllerStick;
    private XRBinding leftControllerStick;


    void Start()
    {
        
    }


    public void RegisterInteraction()
    {
        GameManager.Instance.XRInputRight.bindings.
            Add(new XRBinding(XRButton.Trigger, PressType.End, () => ControllerEventTrigger()));
        GameManager.Instance.XRInputLeft.bindings.
            Add(new XRBinding(XRButton.Trigger, PressType.End, () => ControllerEventTrigger()));
    }

    private void RegisterGrabbing()
    {
        GameManager.Instance.XRInputRight.bindings.
            Add(new XRBinding(XRButton.GripButton, PressType.Begin, () => ControllerEventGrip()));
    }

    private void RegisterSticks()
    {

    }

    public void RegisterButtonEvents()
    {
        XRButton teleportLeft;
        XRButton teleportRight;

        if (GameManager.Instance.OculusInUse)
        {
            teleportLeft = XRButton.SecondaryButton;
            teleportRight = XRButton.PrimaryButton;
        }
        else
        {
            teleportLeft = XRButton.Primary2DAxisClick;
            teleportRight = XRButton.Primary2DAxisClick;
        }

        GameManager.Instance.XRInputRight.bindings.
            Add(new XRBinding(teleportRight, PressType.End, () => ControllerEventButton(false)));
        GameManager.Instance.XRInputRight.bindings.
            Add(new XRBinding(teleportLeft, PressType.End, () => ControllerEventButton(true)));

    }

    public void RemoveButtonEvents()
    {
        if (ControllerEventButton == null) return;
        foreach (Delegate deli in ControllerEventButton.GetInvocationList())
        {
            ControllerEventButton -= (RightControllerButton)deli;
        }

    }

    public void RemoveTriggerEvents()
    {
        if (ControllerEventTrigger == null) return;
        foreach (Delegate deli in ControllerEventTrigger.GetInvocationList())
        {
            ControllerEventTrigger -= (ControllerTrigger)deli;
        }

    }



}
