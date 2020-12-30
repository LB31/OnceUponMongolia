using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DialogManager;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI DialogText;

    public Person SpeakingPerson;

    

    private void Awake()
    {
        CharacterName = transform.Find("NameBox/NameText").GetComponent<TextMeshProUGUI>();
        DialogText = transform.Find("DialogText").GetComponent<TextMeshProUGUI>();

        CharacterName.text = SpeakingPerson.ToString();

        // Testing
        //StartCoroutine(BuildDialog(TypeMessage.dialog, 0));
    }

    

}
