using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PositionChangeTrigger : MonoBehaviour
{
    public Transform NextCam;
    public Transform PreviousCam;

    public Transform Player;

    private bool nextCam;

    private void Start()
    {
        Player = FindObjectOfType<XRRig>().transform;
    }

    private void OnTriggerExit(Collider other)
    {
        Vector3 direction = other.transform.position - transform.position;
        if (Vector3.Dot(transform.forward, direction) > 0)
        {
            print("Front");
            if (!nextCam)
            {
                nextCam = true;
                Player.position = NextCam.position;
            }
            
        }
        if (Vector3.Dot(transform.forward, direction) < 0)
        {
            print("Back");
            if (nextCam)
            {
                nextCam = false;
                Player.position = PreviousCam.position;
            }
                
        }
    }
}
