using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform Target;
    public Vector3 LookAxis;


    void Update()
    {
        transform.LookAt(Target, LookAxis);
    }
}
