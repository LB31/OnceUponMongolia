using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    public TextAsset AllDialogues;
    public List<ItemTexts> Dialogues;
    public float ScrollingSpeed = 0.01f;

    private Dialog currentDialog;


    protected override void Awake()
    {
        base.Awake();

        ParseJson();
    }

    private void Start()
    {
        //StartDialog(Person.Vero, 1, TypeMessage.veroText, QuestType.none, VeroType.grandma);
        //StartDialog(Person.Grandmother, 1, TypeMessage.quest, QuestType.task);
    }

    private void ParseJson()
    {
        VillagerTexts dialogJson = JsonUtility.FromJson<VillagerTexts>(AllDialogues.text);
        Dialogues = new List<ItemTexts>();
        foreach (ItemTexts dialogs in dialogJson.people)
        {
            //dialog.puzzleText = dialog.puzzleText.Replace("\\n", "\n");

            Dialogues.Add(dialogs);

            try
            {
                print(dialogs.veroTexts.girl[1]);
            }
            catch (Exception)
            {
                print("try again");
            }

        }
    }

    public void StartDialog(Person person, int entryNumber,
        TypeMessage messageType = 0, QuestType questType = 0, VeroType veroType = 0)
    {
        // Find according person
        currentDialog = FindObjectsOfType<Dialog>().First(name => name.SpeakingPerson.Equals(person));

        if (person.Equals(Person.Vero))
            BuildVeroDialog(veroType, entryNumber);
        else
            BuildDialog(person, entryNumber, messageType, questType);
    }

    private void BuildDialog(Person person, int entryNumber, TypeMessage messageType,
        QuestType questType = 0)
    {
        ItemTexts itemTexts = Dialogues.
            Find(name => name.characterName == person.ToString());

        List<string> outputText = new List<string>();

        switch (messageType)
        {
            case TypeMessage.dialog:
                outputText = itemTexts.dialogs[entryNumber].text;
                break;
            case TypeMessage.quest:
                if (questType.ToString() == "task")
                    outputText = itemTexts.quests[entryNumber].task;
                else if (questType.ToString() == "hint")
                    outputText = itemTexts.quests[entryNumber].hint;
                else
                    outputText = itemTexts.quests[entryNumber].ready;
                break;
            case TypeMessage.randomAnswer:
                outputText = itemTexts.randomAnswers[entryNumber].answer;
                break;
            default:
                break;
        }

        StartCoroutine(ScrollDialog(outputText));

    }

    private void BuildVeroDialog(VeroType veroType, int entryNumber)
    {
        ItemTexts itemTexts = Dialogues.
            Find(name => name.characterName == Person.Vero.ToString());
        List<string> outputText = new List<string>();
        string output = "";

        switch (veroType)
        {
            case VeroType.hints:
                output = itemTexts.veroTexts.hints[entryNumber];
                break;
            case VeroType.grandma:
                output = itemTexts.veroTexts.grandma[entryNumber];
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

        outputText.Add(output);

        StartCoroutine(ScrollDialog(outputText));
    }

    private IEnumerator ScrollDialog(List<string> outputText)
    {
        currentDialog.DialogText.text = "";
        foreach (char letter in outputText[0]) // TODO replace zero
        {
            currentDialog.DialogText.text += letter;
            yield return new WaitForSeconds(ScrollingSpeed);
        }
    }

}





