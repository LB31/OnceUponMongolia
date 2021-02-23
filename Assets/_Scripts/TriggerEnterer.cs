using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// For Vero entering buildings and stuff
/// </summary>

public class TriggerEnterer : MonoBehaviour
{
    public float FadeSpeed = 0.5f;

    private Transform veroCams;
    private List<Vector3> originCamPositions = new List<Vector3>();
    private List<Vector3> houseCamPositions = new List<Vector3>()
    {
        new Vector3(0, 1, -2), // back
        new Vector3(-2, 1, 0), // left
        new Vector3(0, 1, 2), // front
        new Vector3(2, 1, 0), // right
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("YurteCollider") && other is CapsuleCollider)
            ChangeCams(true);
        if (other.name.Contains("boat"))
        {
            // TODO boat action
            // Fade screen
            // move Vero, HailStone and hit boat to other side
        }
        if (other.name.Contains("water"))
        {
            // Respawn
            StartCoroutine(VisualizeSceneChange(new Vector3(60f, 5f, 103)));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("YurteCollider"))
            ChangeCams(false);
    }

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

    private void PortCharacter(Vector3 newPos)
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = newPos;
        GetComponent<CharacterController>().enabled = true;
    }

    public IEnumerator VisualizeSceneChange(Vector3 newPos)
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

        // TODO something in between
        PortCharacter(newPos);

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
