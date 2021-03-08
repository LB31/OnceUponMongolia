using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerController : EntityController
{
    public bool FollowVero;
    public Transform Goal;

    private NavMeshAgent agent;

    public override void Start()
    {
        Character = transform;

        base.Start();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent)
        {
            agent.speed = Speed;
            if (!Goal)
                Goal = GameManager.Instance.Vero;
        }
    }

    public void UseWheelBarrow(bool use)
    {
        animator.SetBool("useWheelBarrow", use);
        FollowVero = use;
    }

    public void CostumeVeroAsVillager()
    {
        Goal = GameManager.Instance.Player;
        UseWheelBarrow(true);
        agent.enabled = true;
        agent.speed = 2;
        GetComponent<CharacterController>().stepOffset = 1;
    }

    public void ReturnMainVero()
    {
        Goal = null;
        animator.enabled = true;
        UseWheelBarrow(false);
        agent.enabled = false;     
        animator.Play("Idle");
        GetComponent<CharacterController>().stepOffset = 0.4f;
        Destroy(this);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!FollowVero || !agent) return;

        agent.destination = Goal.position;

        animator.SetFloat(velocityHash, agent.velocity.magnitude);

        if (agent.remainingDistance <= agent.stoppingDistance && Time.time > 3 && name.Contains("Vero"))
        {
            if (agent.velocity.sqrMagnitude == 0f && animator.enabled)
            {
                print("arrived");
                animator.enabled = false;
            }
        }
        if (!animator.enabled && agent.velocity.sqrMagnitude > 0)
            animator.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<GirlController>()) return;
        if (GameManager.Instance.NearestVillager != null)
            return;

        GameManager.Instance.NearestVillager = this;
        GetComponent<PlayMakerFSM>().SendEvent("LookAtVero");

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<GirlController>()) return;
        GetComponent<PlayMakerFSM>().SendEvent("IgnoreVero");
        GameManager.Instance.NearestVillager = null;
    }

    public void TriggerFollowingVero(bool follow)
    {
        FollowVero = follow;
    }

}
