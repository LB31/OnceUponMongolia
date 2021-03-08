using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Simulator : MonoBehaviour
{
    public static Vector2 Movement;
    public static UnityEvent ButtonEvent = new UnityEvent();
    private Camera simCam;

    void Awake()
    {
        Camera.main.enabled = false;
        simCam = GetComponentInChildren<Camera>();
        simCam.tag = "MainCamera";
    }

    void Update()
    {
        Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonEvent.Invoke();
            print("space");
        }


    }
}
