using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager Instance;

    public GameObject choicePanel;

    void Awake()
    {
        Instance = this;
        choicePanel.SetActive(false);
    }

    public void OpenChoice()
    {
        choicePanel.SetActive(true);
    }

    public void Choice1()
    {
        choicePanel.SetActive(false);

        DialogueManager.Instance.EndDialogue();

        // 퀴즈 시작
        TwoChoiceRoomManager.Instance.EnterQuizRoom();
    }

    public void Choice2()
    {
        choicePanel.SetActive(false);

        DialogueManager.Instance.EndDialogue();

        DialogueManager.Instance.StartEvent(14);
    }
}