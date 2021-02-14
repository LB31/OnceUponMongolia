using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    public TextAsset AllDialogues;
    public List<ItemTexts> Dialogues;
    public float ScrollingSpeed = 0.01f;

    private Dialog currentDialog;
    // Debug
    public List<string> outputText = new List<string>();
    private int currentTextIndex;

    private IEnumerator runningDialog;
    private bool dialogIsRunning;


    protected override void Awake()
    {
        base.Awake();

        ParseJson();     
    }

    private void ParseJson()
    {
        VillagerTexts dialogJson = JsonUtility.FromJson<VillagerTexts>(AllDialogues.text);
        Dialogues = dialogJson.people;
    }

    public void StartDialog(Person person, int entryNumber,
        TypeMessage messageType = 0, QuestType questType = 0, VeroType veroType = 0)
    {
        // Find according person
        // TODO rework
        currentDialog = FindObjectsOfType<Dialog>().First(name => name.SpeakingPerson.Equals(person));

        currentDialog.CharacterName.text = person.ToString();

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

        //outputText.Clear();

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
                else if (questType.ToString() == "ready")
                    outputText = itemTexts.quests[entryNumber].ready;
                break;
            case TypeMessage.randomAnswer:
                outputText = itemTexts.randomAnswers[entryNumber].answer;
                break;
            default:
                break;
        }

        runningDialog = ScrollDialog();
        StartCoroutine(runningDialog);

    }

    private void BuildVeroDialog(VeroType veroType, int entryNumber)
    {
        ItemTexts itemTexts = Dialogues.
            Find(name => name.characterName == Person.Vero.ToString());

        //outputText.Clear();
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
                output = itemTexts.veroTexts.father[entryNumber];
                break;
            case VeroType.girl:
                output = itemTexts.veroTexts.girl[entryNumber];
                break;
            case VeroType.boy:
                output = itemTexts.veroTexts.boy[entryNumber];
                break;
            case VeroType.itemfound:
                output = itemTexts.veroTexts.itemfound[entryNumber];
                break;
            default:
                break;
        }

        outputText.Add(output);

        runningDialog = ScrollDialog();
        StartCoroutine(runningDialog);
    }

    private IEnumerator ScrollDialog()
    {
        dialogIsRunning = true;
        currentDialog.DialogText.text = "";
        foreach (char letter in outputText[currentTextIndex]) 
        {
            currentDialog.DialogText.text += letter;
            yield return new WaitForSeconds(ScrollingSpeed);
        }
        dialogIsRunning = false;
    }

    public async void ContinueDialog()
    {
        if (!currentDialog) return;

        // When dialog is scrolling, show all
        if (dialogIsRunning)
        {
            StopCoroutine(runningDialog);
            currentDialog.DialogText.text = outputText[currentTextIndex]; // TODO no zero
            dialogIsRunning = false;
        }
        // When there is more to say, show next
        else if(outputText.Count - 1 > currentTextIndex)
        {
            currentTextIndex++;
            runningDialog = ScrollDialog();
            StartCoroutine(runningDialog);
        }
        // Send event, when dialog finished
        else
        {
            await Task.Delay(500);
            currentTextIndex = 0;
            currentDialog.DialogText.text = "";
            currentDialog.gameObject.SetActive(false);
            currentDialog = null;
            PlayMakerFSM.BroadcastEvent("DialogFinished");

        }
    }

}





