using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DialogManager;

public class Dialog : MonoBehaviour
{
    public Person SpeakingPerson;
    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI DialogText;

    public Button YesButton;
    public Button NoButton;

    private void Awake()
    {
        CharacterName.text = SpeakingPerson.ToString();

        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
    }

    

}
