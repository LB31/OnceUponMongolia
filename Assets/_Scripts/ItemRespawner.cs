using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        originPos = transform.localPosition;
        originScale = transform.localScale;
        rg = GetComponent<Rigidbody>();
    }

    public async void ReturnToOriginalPos()
    {
        await Task.Delay(1000);
        transform.localPosition = originPos;
        transform.localScale = originScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When the hit object doesn't have an allowed colision tag
        if(!AllowedCollisionTags.Contains(collision.transform.tag))
        {
            ReturnToOriginalPos();

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
