﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerController : EntityController
{
    public bool FollowVero;

    private NavMeshAgent agent;


    public override void Start()
    {
        Character = transform;

        base.Start();

        agent = GetComponent<NavMeshAgent>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!FollowVero) return;

        agent.destination = GameManager.Instance.Vero.position;


        //if (agent.remainingDistance <= agent.stoppingDistance)
        //{
        //    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        //    {
        //        print("arrived");
        //    }
        //}

        animator.SetFloat(velocityHash, agent.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.ToLower().Contains("vero")) return;
        GameManager.Instance.NearestVillager = this;
        GameManager.Instance.NearestVillager.GetComponent<PlayMakerFSM>().SendEvent("LookAtVero");

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.name.ToLower().Contains("vero")) return;
        GameManager.Instance.NearestVillager.GetComponent<PlayMakerFSM>().SendEvent("IgnoreVero");
        GameManager.Instance.NearestVillager = null;
    }
}
