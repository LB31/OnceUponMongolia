using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.AI;
using static BackgroundMusic;

/// <summary>
/// For Vero entering buildings and stuff
/// </summary>

public class TriggerEnterer : MonoBehaviour
{
    public float FadeSpeed = 0.5f;

    public static GameObject CurrentItemInRange;

    private bool onTempleSide;
    private bool inTrigger;

    private Transform veroCams;
    private List<Vector3> originCamPositions = new List<Vector3>();
    private List<Vector3> houseCamPositions = new List<Vector3>()
    {
        new Vector3(0, 1f, -1.5f), // back
        new Vector3(-1.5f, 1f, 0), // left
        new Vector3(0, 1f, 1.5f), // front
        new Vector3(1.5f, 1f, 0), // right
    };

    private PositionManager posMan;

    private void Start()
    {
        posMan = PositionManager.Instance;

        veroCams = transform.Find("VeroCams");

        foreach (Transform cam in veroCams)
            originCamPositions.Add(cam.localPosition);     
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (inTrigger) return;
        inTrigger = true;

        if (other.tag == "Item"){
            CurrentItemInRange = other.gameObject;
        }
        if (other.name.Contains("YurteCollider") && other is CapsuleCollider)
            ChangeVRCamDistance(true);
        if (other.name.Contains("boat"))
        {
            // New Vero Position
            Location nextLocation = onTempleSide ? Location.Harbor : Location.Temple;
            //StartCoroutine(VisualizeSceneChange(posMan.GetNextTransform(nextLocation)));
            posMan.ChangeVeroPosition(posMan.TeleportCharacter, transform, posMan.GetNextTransform(nextLocation));

            // Change Music
            Music musicType = onTempleSide ? Music.Village : Music.Ruins;
            AudioManager.Instance.ChangeMusic(musicType);

            // New Boat Positon
            await Task.Delay(1000); // wait for black transition
            Vector3 nextBoatPos;
            if (other.name.Contains("Right"))
                nextBoatPos = onTempleSide ? posMan.BoatPositions[0] : posMan.BoatPositions[1];
            else
                nextBoatPos = onTempleSide ? posMan.BoatPositions[2] : posMan.BoatPositions[3];
            other.transform.position = nextBoatPos;

            // Teleport HailStone to Temple side when he is following Vero
            VillagerController hailStone = GameManager.Instance.HailStone;
            if (hailStone.FollowVero)
            {
                float offset = onTempleSide ? -5 : 5;
                Vector3 target = posMan.GetNextTransform(nextLocation).position + new Vector3(offset, 0, 0);
                posMan.TeleportNavAgent(hailStone.transform, target);
                hailStone.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("giveSecondHint").Value = !onTempleSide;
                // Trigger quest markers
                GameManager.Instance.QuestMarkers.First(m => m.MarkerOwner == Person.GreatTiger).MarkerObject.SetActive(!onTempleSide);
                GameManager.Instance.QuestMarkers.First(m => m.MarkerOwner == Person.Boat).MarkerObject.SetActive(onTempleSide);
            }

            onTempleSide = !onTempleSide;
        }
        if (other.name.Contains("water"))
        {
            print("respawn");
            // Respawn
            Location nextLocation = onTempleSide ? Location.Temple : Location.Harbor;
            //StartCoroutine(VisualizeSceneChange(posMan.GetNextTransform(nextLocation)));
            posMan.ChangeVeroPosition(posMan.TeleportCharacter, transform, posMan.GetNextTransform(nextLocation));
        }
        // For quest 3
        if (other.name.Contains("BridgeTrigger"))
        {
            PlayMakerFSM.BroadcastEvent("ContinueStory");
            other.gameObject.SetActive(false);
        }

        inTrigger = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("YurteCollider"))
            ChangeVRCamDistance(false);
        if (other.tag == "Item")
            CurrentItemInRange = null;
    }

    // For entering yurts
    private void ChangeVRCamDistance(bool inHouse)
    {
        for (int i = 0; i < veroCams.childCount; i++)
        {
            if (inHouse)
                veroCams.GetChild(i).localPosition = houseCamPositions[i];
            else
                veroCams.GetChild(i).localPosition = originCamPositions[i];
        }
    }

}




