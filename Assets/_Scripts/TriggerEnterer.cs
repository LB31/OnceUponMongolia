using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.AI;

/// <summary>
/// For Vero entering buildings and stuff
/// </summary>

public class TriggerEnterer : MonoBehaviour
{
    public float FadeSpeed = 0.5f;

    // Teleport Positions
    public List<CustomTransform> VeroPositions;

    public List<Vector3> BoatPositions = new List<Vector3>()
    {
        new Vector3(70f, 2.8f, 97f), // boat right origin
        new Vector3(87f, 2.8f, 93f), // boat right temple
        new Vector3(72f, 2.8f, 103f), // boat left origin
        new Vector3(87f, 2.8f, 100f), // boat left temple
    };

    private bool onTempleSide;
    private bool inTrigger;


    private Transform veroCams;
    private List<Vector3> originCamPositions = new List<Vector3>();
    private List<Vector3> houseCamPositions = new List<Vector3>()
    {
        new Vector3(0, 1.5f, -2), // back
        new Vector3(-2, 1.5f, 0), // left
        new Vector3(0, 1.5f, 2), // front
        new Vector3(2, 1.5f, 0), // right
    };

    private Image sceneChanger;

    private void Start()
    {
        sceneChanger = GameObject.Find("WorldSwitchCanvas")
            .transform.GetChild(0).GetComponent<Image>();

        veroCams = transform.Find("VeroCams");

        foreach (Transform cam in veroCams)
            originCamPositions.Add(cam.localPosition);
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (inTrigger) return;
        inTrigger = true;

        if (other.name.Contains("YurteCollider") && other is CapsuleCollider)
            ChangeCams(true);
        if (other.name.Contains("boat"))
        {
            // TODO boat action

            // New Vero Position
            Location nextLocation = onTempleSide ? Location.Harbor : Location.Temple;
            StartCoroutine(VisualizeSceneChange(GetNextTransform(nextLocation)));

            // New Boat Positon
            await Task.Delay(1000);
            Vector3 nextBoatPos;
            if (other.name.Contains("Right"))
                nextBoatPos = onTempleSide ? BoatPositions[0] : BoatPositions[1];
            else
                nextBoatPos = onTempleSide ? BoatPositions[2] : BoatPositions[3];
            other.transform.position = nextBoatPos;

            // Teleport HailStone when he is following Vero
            VillagerController hailStone = GameManager.Instance.HailStone;
            if (hailStone.FollowVero)
            {
                hailStone.GetComponent<NavMeshAgent>().enabled = false;
                float offset = onTempleSide ? -5 : 5;
                hailStone.transform.position = GetNextTransform(nextLocation).position + new Vector3(offset, 0, 0);
                hailStone.GetComponent<NavMeshAgent>().enabled = true;
                hailStone.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("giveSecondHint").Value = !onTempleSide;
                // Trigger quest markers
                GameManager.Instance.QuestMarkers.First(m => m.MarkerOwner == Person.GreatTiger).MarkerObject.SetActive(!onTempleSide);
                GameManager.Instance.QuestMarkers.First(m => m.MarkerOwner == Person.Boat).MarkerObject.SetActive(onTempleSide);
            }


            onTempleSide = !onTempleSide;
            // move Vero, HailStone and hit boat to other side
        }
        if (other.name.Contains("water"))
        {
            // Respawn
            Location nextLocation = onTempleSide ? Location.Temple : Location.Harbor;
            StartCoroutine(VisualizeSceneChange(GetNextTransform(nextLocation)));
        }

        inTrigger = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("YurteCollider"))
            ChangeCams(false);
    }

    private Transform GetNextTransform(Location location)
    {
        CustomTransform nextTransform = VeroPositions.Find(l => l.Location == location);
        Transform nextVeroPos = new GameObject().transform;
        nextVeroPos.position = nextTransform.Position;
        nextVeroPos.rotation = Quaternion.Euler(nextTransform.Rotation);
        return nextVeroPos;
    }

    // For entering yurts
    private void ChangeCams(bool inHouse)
    {
        for (int i = 0; i < veroCams.childCount; i++)
        {
            if (inHouse)
                veroCams.GetChild(i).localPosition = houseCamPositions[i];
            else
                veroCams.GetChild(i).localPosition = originCamPositions[i];
        }
    }

    private void TeleportCharacter(Transform trans)
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = trans.position;
        transform.rotation = trans.localRotation;
        GetComponent<CharacterController>().enabled = true;
    }
    private void TeleportCharacter(Vector3 pos)
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = pos;
        GetComponent<CharacterController>().enabled = true;
    }

    public IEnumerator VisualizeSceneChange(Transform newPos, Vector3? pos = null)
    {
        sceneChanger.enabled = true;
        Color color = sceneChanger.color;
        for (float i = 0; i <= 1; i += Time.deltaTime * FadeSpeed)
        {
            color.a = i;
            sceneChanger.color = color;
            yield return null;
        }

        color.a = 1;
        sceneChanger.color = color;

        // Do something in between
        if (pos == null)
            TeleportCharacter(newPos);
        else
            TeleportCharacter(pos.Value);

        yield return new WaitForSeconds(1);

        for (float i = 1; i >= 0; i -= Time.deltaTime * FadeSpeed)
        {
            color.a = i;
            sceneChanger.color = color;
            yield return null;
        }

        sceneChanger.enabled = false;
    }
}

[Serializable]
public class CustomTransform
{
    public Location Location;
    public Vector3 Position;
    public Vector3 Rotation;
}

public enum Location
{
    Temple,
    Harbor,
}
