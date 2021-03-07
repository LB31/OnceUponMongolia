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

    public enum GameState
    {
        MoveFreely,
        ControlGirl,
        PortIntoCharacter,
        PassiveFirstPerson
    };
    public GameState StartState;

    private List<MonoBehaviour> allComponents = new List<MonoBehaviour>();

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        continuousMovement = GetComponent<ContinuousMovement>();
        snapTurnProvider = GetComponent<DeviceBasedSnapTurnProvider>();
        positionChanger = GetComponent<PositionChanger>();
        girlController = FindObjectOfType<GirlController>();

        //allComponents.Add(characterController);
        allComponents.Add(continuousMovement);
        allComponents.Add(snapTurnProvider);
        allComponents.Add(positionChanger);
        allComponents.Add(girlController);

        DisableAllComponents();

        switch (StartState)
        {
            case GameState.MoveFreely:
                MoveFreely();
                break;
            case GameState.ControlGirl:
                ControlGirl();
                break;
            case GameState.PortIntoCharacter:
                PortIntoCharacter();
                break;
            case GameState.PassiveFirstPerson:
                PassiveFirstPerson();
                break;
            default:
                break;
        }
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

    public void PortIntoCharacter()
    {
        DisableAllComponents();

        positionChanger.enabled = true;
    }

    public void PassiveFirstPerson()
    {
        DisableAllComponents();
        // TODO
    }



}
