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

    // Debug
    public Transform CurrentVeroPosition;

    private List<Transform> currentPositions;
    private int currentPosition;

    //private XRBinding teleportBindingLeft;
    private XRBinding teleportBindingRightA;
    private XRBinding teleportBindingRightB;


    void Awake()
    {
        currentPositions = AreaPositions[0].PlayerPositions;
        currentPosition = currentPositions.IndexOf(AreaPositions[0].StartPos);
        CurrentVeroPosition = currentPositions[currentPosition];
        if (enabled)
            ChangeTransform();
        //RegisterButtonEvents();
    }

    private void OnEnable()
    {
        RegisterButtonEvents();
    }

    private void OnDisable()
    {
        RemoveButtonEvents();
    }

    public void RemoveButtonEvents()
    {
        if (teleportBindingRightA != null)
        {
            //GameManager.Instance.XRInputLeft.bindings.Remove(teleportBindingLeft);
            GameManager.Instance.XRInputRight.bindings.Remove(teleportBindingRightA);
            GameManager.Instance.XRInputRight.bindings.Remove(teleportBindingRightB);
        }
    }

    public void RegisterButtonEvents()
    {
        RemoveButtonEvents();

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
            Add(teleportBindingRightB = new XRBinding(teleportLeft, PressType.End, () => Teleport(false)));
        GameManager.Instance.XRInputRight.bindings.
            Add(teleportBindingRightA = new XRBinding(teleportRight, PressType.End, () => Teleport(true)));
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
        transform.rotation = currentPositions[currentPosition].localRotation;
        CurrentVeroPosition = currentPositions[currentPosition];
    }

    public void ChangeArea(string name)
    {

    }

    public void RecenterPlayer()
    {

    }

    public void TestMove()
    {
        StartCoroutine(MoveIntoCharacter(GameObject.Find("GurlToMove").transform));
    }

    public IEnumerator MoveIntoCharacter(Transform goal)
    {
        while (Vector3.Distance(transform.position, goal.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goal.position, Time.deltaTime * 5);
            yield return null;
        }

        goal.gameObject.SetActive(false);
    }

}

[Serializable]
public class AreaPositions
{
    public string AreaName;
    public Transform StartPos;
    public List<Transform> PlayerPositions;

}
