using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class PositionChangeTrigger : MonoBehaviour
{
    public Transform NextCam;
    public Transform PreviousCam;
    public float FadeDuration = 2;

    private Transform Player;
    private Image SceneChanger;
    private bool nextCam;
    private Vector3 direction;

    private void Start()
    {
        Player = FindObjectOfType<XRRig>().transform;
        SceneChanger = GameObject.Find("WorldSwitchCanvas")
            .transform.GetChild(0).GetComponent<Image>();
    }

    private void OnTriggerExit(Collider other)
    {
        direction = other.transform.position - transform.position;
        StartCoroutine(VisualizeSceneChange());
    }

    public IEnumerator VisualizeSceneChange()
    {
        SceneChanger.enabled = true;
        Color color = SceneChanger.color;
        for (float i = 0; i <= 1; i += Time.deltaTime * FadeDuration)
        {
            color.a = i;
            SceneChanger.color = color;
            yield return null;
        }

        ChangePosition();
        yield return new WaitForSeconds(1);

        for (float i = 1; i >= 0; i -= Time.deltaTime * FadeDuration)
        {
            color.a = i;
            SceneChanger.color = color;
            yield return null;
        }

        SceneChanger.enabled = false;
    }

    private void ChangePosition()
    {
        if (Vector3.Dot(transform.forward, direction) > 0)
        {
            print("Front");
            if (!nextCam)
            {
                nextCam = true;
                Player.position = NextCam.position;
                Player.rotation = NextCam.rotation;
            }

        }
        if (Vector3.Dot(transform.forward, direction) < 0)
        {
            print("Back");
            if (nextCam)
            {
                nextCam = false;
                Player.position = PreviousCam.position;
                Player.rotation = PreviousCam.rotation;
            }

        }
    }

}
