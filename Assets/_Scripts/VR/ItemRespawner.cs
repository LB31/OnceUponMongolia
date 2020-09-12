using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawner : MonoBehaviour
{
    public string AllowedCollisionTag;

    private Vector3 originPos;
    private Rigidbody rg;

    private void Start()
    {
        originPos = transform.position;
        rg = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag(AllowedCollisionTag))
        {
            transform.position = originPos;
            rg.useGravity = false;
            rg.velocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }
}
