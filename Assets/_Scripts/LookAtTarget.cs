using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform Target;
    public Vector3 LookAxis;
    public bool ForDialogs;

    private void Start()
    {
        if (Target == null)
            Target = Camera.main.transform;
        if (LookAxis == Vector3.zero)
            LookAxis = Vector3.up;
    }

    void Update()
    {
        transform.LookAt(Target, LookAxis);
        if (ForDialogs)
            transform.rotation *= Quaternion.Euler(0, 180, 0);

    }
}
