using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GamblingManager : MonoBehaviour
{ 
    public Transform DiceParent;
    private List<string> OracleAnswers;
    private TextMeshProUGUI OracleText;
    [SerializeField] private int stonesInside;
    [SerializeField] private int answerCount;

    private void Start()
    {
        answerCount = DialogManager.Instance.Dialogues.First(dialog => dialog.characterName.Equals("SevenStar")).quests.Count;
    }

    private async void ShowOracleMessage()
    {
        var granny = GameManager.Instance.GrannyNormal;
        GameManager.Instance.NearestVillager = granny;
        int answer = Random.Range(0, answerCount);
        granny.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("diceAnswer").Value = answer;
        PlayMakerFSM.BroadcastEvent("DiceLevel");

        await Task.Delay(2000);

        foreach (Transform dice in DiceParent)
        {
            dice.GetComponent<ItemRespawner>().ReturnToOriginalPos();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Dice"))
        {
            stonesInside++;
        }

        if (stonesInside == 4)
        {
            ShowOracleMessage();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Dice"))
        {
            stonesInside--;
        }
    }

}
