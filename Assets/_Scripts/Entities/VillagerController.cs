using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : EntityController
{
    public float LookAtDistance = 5;
    //public float InteractionDistance = 2;

    private bool lookingAtVero;

    public override void Start()
    {
        Character = transform;

        base.Start();
    }

    private void Update()
    {
        //float distToVero = Vector3.Distance(transform.position, GameManager.Instance.Vero.position);

        //// Interaction Distance
        //if (distToVero < LookAtDistance)
        //{
        //    GameManager.Instance.NearestVillager = this;
        //    if (!lookingAtVero)
        //    {
        //        GameManager.Instance.NearestVillager.GetComponent<PlayMakerFSM>().SendEvent("LookAtVero");
        //        lookingAtVero = true;
        //    }

        //}
        //if (distToVero > LookAtDistance && GameManager.Instance.NearestVillager == this)
        //{
        //    if (lookingAtVero)
        //    {
        //        GameManager.Instance.NearestVillager.GetComponent<PlayMakerFSM>().SendEvent("IgnoreVero");
        //        lookingAtVero = false;
        //    }
        //    GameManager.Instance.NearestVillager = null;

        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.NearestVillager = this;
        GameManager.Instance.NearestVillager.GetComponent<PlayMakerFSM>().SendEvent("LookAtVero");

    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.NearestVillager.GetComponent<PlayMakerFSM>().SendEvent("IgnoreVero");
        GameManager.Instance.NearestVillager = null;
    }
}
