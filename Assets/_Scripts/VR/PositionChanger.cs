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


    void Start()
    {
        currentPositions = AreaPositions[0].PlayerPositions;
        currentPosition = currentPositions.IndexOf(AreaPositions[0].StartPos);
        ChangeTransform();
        RegisterButtonEvents();
    }

    private void RegisterButtonEvents()
    {
        XRButton teleportButton = GameManager.Instance.OculusInUse ? XRButton.PrimaryButton : XRButton.Primary2DAxisClick;

        GameManager.Instance.XRInputLeft.bindings.
            Add(new XRBinding(teleportButton, PressType.End, () => Teleport(false)));
        GameManager.Instance.XRInputRight.bindings.
            Add(new XRBinding(teleportButton, PressType.End, () => Teleport(true)));
    }

    public void Teleport(bool right)
    {
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
