using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public TextAsset AllDialogues;

    public List<ItemTexts> Dialogues;

    public enum Person
    {
        Vero,
        Grandmother,
        Father,
        Girl,
        Boy
    }

    public enum MessageType
    {
        dialog,
        quest,
        randomAnswer,
        veroText
    }

    public enum QuestType
    {
        task,
        hint,
        ready
    }

    private void Awake()
    {
        if (Instance)
            return;
        else
            Instance = this;

        ParseJson();
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

}

[Serializable]
public class VillagerTexts
{
    public List<ItemTexts> people;
}

[Serializable]
public class ItemTexts
{
    public int id;
    public string characterName;
    public List<Dialogs> dialogs;
    public List<QuestsTexts> quests;
    public List<RandomAnswer> randomAnswers;
    public VeroTexts veroTexts;
}

[Serializable]
public class Dialogs
{
    public List<string> text;
}

[Serializable]
public class QuestsTexts
{
    public List<string> task;
    public List<string> hint;
    public List<string> ready;
}

[Serializable]
public class RandomAnswer
{
    public List<string> answer;
}

[Serializable]
public class VeroTexts
{
    public List<string> hints;
    public List<string> grandma;
    public List<string> father;
    public List<string> girl;
    public List<string> boy;
    // 0 = itemX, 1 = itemY ..
    public List<string> itemfound;
}

public enum VeroType
{
    hints,
    grandma,
    father,
    girl,
    boy,
    itemfound
}

