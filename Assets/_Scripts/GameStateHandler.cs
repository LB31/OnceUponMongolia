using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameStateHandler : MonoBehaviour
{
    public CharacterController characterController;
    public ContinuousMovement continuousMovement;
    public DeviceBasedSnapTurnProvider snapTurnProvider;
    public PositionChanger positionChanger;
    public GirlController girlController;

    private List<MonoBehaviour> allComponents = new List<MonoBehaviour>();

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        continuousMovement = GetComponent<ContinuousMovement>();
        snapTurnProvider = GetComponent<DeviceBasedSnapTurnProvider>();
        positionChanger = GetComponent<PositionChanger>();
        girlController = GetComponent<GirlController>();

        //allComponents.Add(characterController);
        allComponents.Add(continuousMovement);
        allComponents.Add(snapTurnProvider);
        allComponents.Add(positionChanger);
        allComponents.Add(girlController);

        DisableAllComponents();

        // test
        MoveFreely();
    }

    public void DisableAllComponents(bool snapTurn = true)
    {
        foreach (MonoBehaviour component in allComponents)
        {
            component.enabled = false;
        }
        characterController.enabled = false;
        snapTurnProvider.enabled = snapTurn;
    }

    public void MoveFreely()
    {
        DisableAllComponents();

        characterController.enabled = true;
        continuousMovement.enabled = true;
    }

    public void ControlGirl()
    {
        DisableAllComponents();

        positionChanger.enabled = true;
        girlController.enabled = true;
    }



}
