using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool OculusInUse;

    private void Awake() {
        if (Instance)
            return;
        Instance = this;
    }
}
