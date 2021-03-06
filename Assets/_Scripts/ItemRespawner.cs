﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemRespawner : MonoBehaviour
{
    public List<string> AllowedCollisionTags = new List<string> { "Table", "Pot", "Plate" };
    public bool YurtLevel;
    public bool CookingLevel;

    private Vector3 originPos;
    private Vector3 originScale;
    private Rigidbody rg;
    private XRGrabInteractable grabInteractable;

    private void Start()
    {
        originPos = transform.localPosition;
        originScale = transform.localScale;
        rg = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable)
        {
            grabInteractable.onSelectEntered.AddListener(ObjectGrabbed);
            grabInteractable.onSelectExited.AddListener(ObjectReleased);

            if (YurtLevel)
            {
                var snappy = FindObjectOfType<SnapController>();
                grabInteractable.onSelectEntered.AddListener(delegate { snappy.RegisterGrabbedOjbect(gameObject); });
            }
        }

    }

    private void ObjectGrabbed(XRBaseInteractor arg0)
    {
        Debug.LogError("grabbed");
        //rg.useGravity = true;
        //rg.isKinematic = false;
        //GetComponent<Collider>().isTrigger = false;
    }

    private void ObjectReleased(XRBaseInteractor arg0)
    {
        Debug.LogError("released");
        // Allow object to fall and to to collide
        rg.useGravity = true;
        rg.isKinematic = false;
        GetComponent<Collider>().isTrigger = false;

        //ReturnToOriginalPos();
    }



    public async void ReturnToOriginalPos()
    {
        if (!gameObject) return; // TODO test if this helps
        await Task.Delay(1000);
        transform.localPosition = originPos;
        transform.localScale = originScale;

        rg.velocity = Vector3.zero;
        rg.angularVelocity = Vector3.zero;



    }

    private void OnCollisionEnter(Collision collision)
    {
        // When the hit object doesn't have an allowed colision tag
        if (!AllowedCollisionTags.Contains(collision.transform.tag))
        {
            ReturnToOriginalPos();
        }
        else if (collision.transform.CompareTag("Pot"))
        {
            FindObjectOfType<CookingController>().PutFoodInPot(gameObject.name);
            Destroy(gameObject);
        }
        else
        {
            rg.useGravity = false;
            rg.isKinematic = true;

            var collider = GetComponent<Collider>();
            if (CookingLevel)
            {
                if (collider) collider.isTrigger = true;
            }
            if (YurtLevel)
            {
                if (collider) collider.isTrigger = false;

                var renderer = GetComponent<Renderer>();
                if (renderer) renderer.enabled = false;
            }
        }
    }
}
