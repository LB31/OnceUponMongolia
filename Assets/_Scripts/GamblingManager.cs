using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamblingManager : MonoBehaviour
{
    public List<string> OracleAnswers;
    public TextMeshProUGUI OracleText;
    private int stonesInside;

    private void ShowOracleMessage()
    {
        int answer = Random.Range(0, OracleAnswers.Count);
        OracleText.text = OracleAnswers[answer];
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
