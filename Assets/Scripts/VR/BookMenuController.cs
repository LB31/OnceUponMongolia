﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BookMenuController : MonoBehaviour
{
    public SteamVR_Action_Boolean BookMenu;
    public SteamVR_Action_Boolean Map;
    public SteamVR_Action_Boolean PageLeft;
    public SteamVR_Action_Boolean PageRight;

    public SteamVR_Action_Vibration Vibration;

    [HideInInspector]
    public bool InBookMenu;

    // Start is called before the first frame update
    void Start()
    {
        BookMenu.AddOnStateDownListener(OpenBookMenu, SteamVR_Input_Sources.RightHand);
        Map.AddOnStateDownListener(ShowMap, SteamVR_Input_Sources.LeftHand);
        PageLeft.AddOnStateDownListener(TurnPageLeft, SteamVR_Input_Sources.LeftHand);
        PageRight.AddOnStateDownListener(TurnPageRight, SteamVR_Input_Sources.RightHand);
    }

    private void TurnPageRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        StartCoroutine(Pulse(1, 1, 5, 1, fromSource));
    }

    private void TurnPageLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        throw new NotImplementedException();
    }

    private void ShowMap(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        throw new NotImplementedException();
    }

    private void OpenBookMenu(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        throw new NotImplementedException();
    }

    private IEnumerator Pulse(int repeats, float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        for (int i = 0; i < repeats; i++)
        {
            Vibration.Execute(0, duration, frequency, amplitude, source);
            yield return new WaitForSeconds(0.1f);
        }

    }

}
