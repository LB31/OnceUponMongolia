﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool ShowController;
    public InputDeviceCharacteristics ControllerCharacteristics;
    public List<GameObject> ControllerPrefabs;
    public GameObject HandModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHand;
    private Animator handAnimator;

    private void Awake()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(ControllerCharacteristics, devices);

        if (devices.Count == 0) return;
        targetDevice = devices[0];

        if (targetDevice.name.ToLower().Contains("oculus"))
            GameManager.Instance.OculusInUse = true;

        if (targetDevice.name.ToLower().Contains("left"))
            GameManager.Instance.LeftCon = targetDevice;
        if (targetDevice.name.ToLower().Contains("left"))
            GameManager.Instance.RightCon = targetDevice;

        // Selecting Josystick
        if (GameManager.Instance.OculusInUse)
            GameManager.Instance.Axis2D = CommonUsages.primary2DAxis;
        else
            GameManager.Instance.Axis2D = CommonUsages.secondary2DAxis;

        GameManager.Instance.ChangeHeadsetDependencies();

        GameObject prefab = ControllerPrefabs.Find(controller => controller.name == targetDevice.name);

        if (prefab)
            spawnedController = Instantiate(prefab, transform);
        // Use the default controller
        else
            spawnedController = Instantiate(ControllerPrefabs[0], transform);

        if (!ShowController)
        {
            spawnedHand = Instantiate(HandModelPrefab, transform);
            handAnimator = spawnedHand.GetComponent<Animator>();
            spawnedController.SetActive(false);
        }
        
    }

    private void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerVal))     
            handAnimator.SetFloat("Trigger", triggerVal);
        else
            handAnimator.SetFloat("Trigger", 0);

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripVal))
            handAnimator.SetFloat("Grip", gripVal);
        else
            handAnimator.SetFloat("Grip", 0);
    }

    void Update()
    {
        if (!targetDevice.isValid)
            TryInitialize();
        else if (!ShowController)
            UpdateHandAnimation();

        //targetDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool primaryButtonVal);
        //if (primaryButtonVal)
        //    print("Pressing menu");

        //targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerVal);
        //if (triggerVal > 0.5f)
        //    print("trigger " + triggerVal);

        //targetDevice.TryGetFeatureValue(CommonUsages.secondary2DAxis, out Vector2 trigger2DVal);
        //if (trigger2DVal != Vector2.zero)
        //    print("2d axis" + trigger2DVal);
    }
}
