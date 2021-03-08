using TMPro;
using UnityEngine;
using UnityEngine.UI;


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

        //YesButton.transform.parent.gameObject.SetActive(true);
        YesButton.onClick.AddListener(() => SendDialogAnswer("answerYes"));
        NoButton.onClick.AddListener(() => SendDialogAnswer("answerNo"));
        //YesButton.transform.parent.gameObject.SetActive(false);

    }

    private void SendDialogAnswer(string answerEvent)
    {
        //GameManager.Instance.NearestVillager.GetComponent<PlayMakerFSM>().SendEvent(answerEvent);
        PlayMakerFSM.BroadcastEvent(answerEvent);
    }



}
