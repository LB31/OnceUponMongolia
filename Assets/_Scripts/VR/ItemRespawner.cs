using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawner : MonoBehaviour
{
    public List<string> AllowedCollisionTags;
    public bool JurteLevel;

    private Vector3 originPos;
    private Vector3 originScale;
    private Rigidbody rg;

    private void Start()
    {
        originPos = transform.position;
        originScale = transform.localScale;
        rg = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When the hit object doesn't have an allowed colision tag
        if(!AllowedCollisionTags.Contains(collision.transform.tag))
        {
            transform.position = originPos;
            transform.localScale = originScale;

            if (JurteLevel)
            {
                rg.useGravity = false;
                rg.isKinematic = true;
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
