using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawner : MonoBehaviour
{
    public List<string> AllowedCollisionTags;
    public bool JurteLevel;

    private Vector3 originPos;
    private Rigidbody rg;

    private void Start()
    {
        originPos = transform.position;
        rg = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!AllowedCollisionTags.Contains(collision.transform.tag))
        {
            transform.position = originPos;
            if (JurteLevel)
            {
                rg.useGravity = false;
                rg.velocity = Vector3.zero;
                rg.angularVelocity = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
        }
    }
}
