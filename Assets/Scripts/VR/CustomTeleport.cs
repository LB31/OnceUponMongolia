using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CustomTeleport : MonoBehaviour
{
    private Transform Player;

    public SteamVR_Action_Boolean TeleportLeftButton;
    public SteamVR_Action_Boolean TeleportRightButton;

    // Testing
    public Transform[] TeleportPoints = new Transform[2];

    private void Start()
    {
        Player = FindObjectOfType<Player>().transform;

        TeleportLeftButton.AddOnStateDownListener(Teleport, SteamVR_Input_Sources.LeftHand);
        TeleportRightButton.AddOnStateDownListener(Teleport, SteamVR_Input_Sources.RightHand);

    }

    private void Teleport(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Transform newPos = fromSource == 
            SteamVR_Input_Sources.LeftHand || TeleportPoints.Length == 1 ? TeleportPoints[0] : TeleportPoints[1];

        StartCoroutine(Move(newPos.position));

    }

    IEnumerator Move(Vector3 pos)
    {
        while (Vector3.Distance(Player.position, pos) > 0.1f)
        {
            Player.position = Vector3.MoveTowards(Player.position, pos, Time.deltaTime * 100);
            yield return null;
        }
    }

}
