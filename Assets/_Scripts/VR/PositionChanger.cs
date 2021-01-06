using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class PositionChanger : MonoBehaviour
{
    public List<AreaPositions> AreaPositions;

    private List<Transform> currentPositions;
    private int currentPosition;

    private XRBinding teleportBindingLeft;
    private XRBinding teleportBindingRight;


    void Start()
    {
        currentPositions = AreaPositions[0].PlayerPositions;
        currentPosition = currentPositions.IndexOf(AreaPositions[0].StartPos);
        ChangeTransform();
        //RegisterButtonEvents();
    }

    private void OnEnable()
    {
        RegisterButtonEvents();
    }

    private void OnDisable()
    {
        if (teleportBindingLeft != null)
        {
            GameManager.Instance.XRInputLeft.bindings.Remove(teleportBindingLeft);
            GameManager.Instance.XRInputRight.bindings.Remove(teleportBindingRight);
        }
    }

    private void RegisterButtonEvents()
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
            Add(teleportBindingRight = new XRBinding(teleportLeft, PressType.End, () => Teleport(false)));
        GameManager.Instance.XRInputRight.bindings.
            Add(teleportBindingRight = new XRBinding(teleportRight, PressType.End, () => Teleport(true)));
    }

    public void Teleport(bool right)
    {
        if (!GameManager.Instance.OculusInUse)
        {
            float offsetTouch = 0.4f;
            GameManager.Instance.RightCon.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightTouchpad);
            if (rightTouchpad.y < -offsetTouch)
                right = false;
            else if (rightTouchpad.y > offsetTouch)
                right = true;
            else
                return;
        }

        if (right)
        {
            currentPosition++;
            if (currentPosition > currentPositions.Count - 1)
                currentPosition = 0;
        }
        else
        {
            currentPosition--;
            if (currentPosition < 0)
                currentPosition = currentPositions.Count - 1;
        }

        ChangeTransform();
    }

    private void ChangeTransform()
    {
        transform.position = currentPositions[currentPosition].position;
        transform.rotation = currentPositions[currentPosition].rotation;
    }

    public void ChangeArea(string name)
    {

    }

    public void RecenterPlayer()
    {

    }

}

[Serializable]
public class AreaPositions
{
    public string AreaName;
    public Transform StartPos;
    public List<Transform> PlayerPositions;

}
