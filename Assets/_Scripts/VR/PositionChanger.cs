using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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

    private Volume volume;
    private LensDistortion lensDistortion;

    void Awake()
    {
        currentPositions = AreaPositions[0].PlayerPositions;
        currentPosition = currentPositions.IndexOf(AreaPositions[0].StartPos);
        CurrentVeroPosition = currentPositions[currentPosition];
        if (enabled)
            ChangeTransform();
        //RegisterButtonEvents();

        volume = FindObjectOfType<Volume>();
        volume.profile.TryGet(out lensDistortion);
    }

    private void OnEnable()
    {
        XRControls.Instance.ControllerEventButton -= Teleport;
        XRControls.Instance.ControllerEventButton += Teleport;

        XRControls.Instance.TestLength();

        print("added teleport");
    }

    private void OnDisable()
    {
        XRControls.Instance.RemoveButtonEvents();
    }



    public void Teleport(bool left)
    {
        // WMR Headset
        if (!GameManager.Instance.OculusInUse)
        {
            float offsetTouch = 0.4f;
            GameManager.Instance.RightCon.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightTouchpad);

            if (rightTouchpad.y < -offsetTouch)
                left = false;
            else if (rightTouchpad.y > offsetTouch)
                left = true;
            else
                return;
        }

        if (left)
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
        float distance = Mathf.Infinity;
        while (distance > 0.1f)
        {
            distance = Vector3.Distance(transform.position, goal.position);
            transform.position = Vector3.MoveTowards(transform.position, goal.position, Time.deltaTime * 5);

            lensDistortion.intensity.value = 1 - (1 / distance);

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
