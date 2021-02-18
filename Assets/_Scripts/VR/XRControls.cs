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

    private XRBinding[] bindingsTrigger = new XRBinding[2];
    private XRBinding[] bindingsButtons = new XRBinding[2];

    void Start()
    {

    }


    public void RegisterInteraction()
    {
        if (bindingsTrigger[0] != null) return;

        GameManager.Instance.XRInputRight.bindings.
            Add(bindingsTrigger[0] = new XRBinding(XRButton.Trigger, PressType.End, () => ControllerEventTrigger()));
        GameManager.Instance.XRInputLeft.bindings.
            Add(bindingsTrigger[1] = new XRBinding(XRButton.Trigger, PressType.End, () => ControllerEventTrigger()));
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
        if (bindingsButtons[0] != null) return;

        XRButton teleportLeft;
        XRButton teleportRight;

        if (GameManager.Instance.OculusInUse)
        {
            teleportLeft = XRButton.SecondaryButton;
            teleportRight = XRButton.PrimaryButton;
        }
        // WMR Headset
        else
        {
            // actually not used here
            teleportLeft = XRButton.Primary2DAxisClick;
            
            teleportRight = XRButton.Primary2DAxisClick;
        }

        GameManager.Instance.XRInputRight.bindings.
            Add(bindingsButtons[0] = new XRBinding(teleportRight, PressType.End, () => ControllerEventButton(false)));
        
        if (GameManager.Instance.OculusInUse)
        {
            GameManager.Instance.XRInputRight.bindings.
                Add(bindingsButtons[1] = new XRBinding(teleportLeft, PressType.End, () => ControllerEventButton(true)));
        }
            

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
