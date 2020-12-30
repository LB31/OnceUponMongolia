using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DialogManager;

public class Dialog : MonoBehaviour
{
    public Person SpeakingPerson;
    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI DialogText;

    private void Awake()
    {
        //CharacterName = transform.Find("NameBox/NameText").GetComponent<TextMeshProUGUI>();
        //DialogText = transform.Find("DialogText").GetComponent<TextMeshProUGUI>();

        CharacterName.text = SpeakingPerson.ToString();
    }

    

}
