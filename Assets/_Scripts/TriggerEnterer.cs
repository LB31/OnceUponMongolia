using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For Vero entering buildings and stuff
/// </summary>

public class TriggerEnterer : MonoBehaviour
{
    private Transform veroCams;
    private List<Vector3> originCamPositions = new List<Vector3>();
    private List<Vector3> houseCamPositions = new List<Vector3>()
    {
        new Vector3(0, 1, -2), // back
        new Vector3(-2, 1, 0), // left
        new Vector3(0, 1, 2), // front
        new Vector3(2, 1, 0), // right
    };

    private void Start()
    {
        veroCams = transform.Find("VeroCams");

        foreach (Transform cam in veroCams)
            originCamPositions.Add(cam.localPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("YurteCollider"))
            ChangeCams(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("YurteCollider"))
            ChangeCams(false);
    }

    private void ChangeCams(bool inHouse)
    {
        for (int i = 0; i < veroCams.childCount; i++)
        {
            if (inHouse)
                veroCams.GetChild(i).localPosition = houseCamPositions[i];
            else
                veroCams.GetChild(i).localPosition = originCamPositions[i];
        }
    }
}
