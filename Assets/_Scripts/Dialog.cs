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

    public float ScrollingSpeed = 0.01f;

    private void Start()
    {
        CharacterName = transform.Find("NameBox/NameText").GetComponent<TextMeshProUGUI>();
        DialogText = transform.Find("DialogText").GetComponent<TextMeshProUGUI>();

        CharacterName.text = SpeakingPerson.ToString();

        // Testing
        StartCoroutine(BuildDialog(MessageType.dialog, 0));
    }

    private IEnumerator BuildDialog(MessageType messageType, int entryNumber, 
        QuestType questType = 0, VeroType veroType = 0)
    {
        ItemTexts itemTexts = Instance.Dialogues.
            Find(name => name.characterName == SpeakingPerson.ToString());

        List<string> outputText = new List<string>();

        switch (messageType)
        {
            case MessageType.dialog:
                outputText = itemTexts.dialogs[entryNumber].text;
                break;
            case MessageType.quest:
                if (questType.ToString() == "task")
                    outputText = itemTexts.quests[entryNumber].task;
                else if(questType.ToString() == "hint")
                    outputText = itemTexts.quests[entryNumber].hint;
                else
                    outputText = itemTexts.quests[entryNumber].ready;
                break;
            case MessageType.randomAnswer:
                outputText = itemTexts.randomAnswers[entryNumber].answer;
                break;
            case MessageType.veroText:
                switch (veroType)
                {
                    case VeroType.hints:
                        outputText = itemTexts.veroTexts.hints;
                        break;
                    case VeroType.grandma:
                        break;
                    case VeroType.father:
                        break;
                    case VeroType.girl:
                        break;
                    case VeroType.boy:
                        break;
                    case VeroType.itemfound:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        DialogText.text = "";
        foreach (char letter in outputText[0])
        {
            DialogText.text += letter;
            yield return new WaitForSeconds(ScrollingSpeed);
        }

    }

}
