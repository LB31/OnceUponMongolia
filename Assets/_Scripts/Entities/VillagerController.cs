using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : EntityController
{
    public float InteractionDistance = 2;

    public override void Start()
    {
        Character = transform;

        base.Start();
    }

    private void Update()
    {
        float distToVero = Vector3.Distance(transform.position, GameManager.Instance.Vero.position);
        if (distToVero < InteractionDistance)
            GameManager.Instance.NearestVillager = this;
        if (distToVero > InteractionDistance && GameManager.Instance.NearestVillager == this)
        {
            GameManager.Instance.NearestVillager = null;
        }
    }
}
