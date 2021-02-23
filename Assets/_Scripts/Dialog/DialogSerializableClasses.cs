using System;
using System.Collections.Generic;

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
    public List<RandomAnswers> randomAnswers;
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
    public List<string> hintSecond;
    public List<string> ready;
}

[Serializable]
public class RandomAnswers
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
