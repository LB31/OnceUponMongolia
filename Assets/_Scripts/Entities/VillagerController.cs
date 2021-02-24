using System.Collections;
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
        if (agent)
            agent.speed = Speed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!FollowVero || !agent) return;

        agent.destination = GameManager.Instance.Vero.position;

        animator.SetFloat(velocityHash, agent.velocity.magnitude);

        //if (agent.remainingDistance <= agent.stoppingDistance)
        //{
        //    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        //    {
        //        print("arrived");
        //    }
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.ToLower().Contains("vero")) return;
        if (GameManager.Instance.NearestVillager != null)
            return;
        GameManager.Instance.NearestVillager = this;
        GetComponent<PlayMakerFSM>().SendEvent("LookAtVero");

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.name.ToLower().Contains("vero")) return;
        GetComponent<PlayMakerFSM>().SendEvent("IgnoreVero");
        GameManager.Instance.NearestVillager = null;
    }

    public void TriggerFollowingVero(bool follow)
    {
        FollowVero = follow;
    }

}
