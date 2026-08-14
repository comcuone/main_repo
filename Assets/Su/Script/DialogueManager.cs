using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject playerPanel;
    public GameObject npcPanel;

    public TMP_Text Name;
    public TMP_Text Dialogue;

    [Header("NPC State")]
    public NPCInteraction SecondStateNPC;
    private Dictionary<int, List<DialogueLine>> dialogueTable;

    private int currentEvent = 0;
    private int currentLine = 0;

    public bool IsDialoguePlaying { get; private set; }

    private GameObject currentPortrait;
    private bool waitingForEndAction = false;

    public GameObject problem1Explanagtion;
    public GameObject problem2Explanation;

    private bool showingExplanation = false;
    private GameObject currentExplanation;

    private bool event13Played = false;

    [SerializeField] private GameObject item;

    private void Awake()
    {
        Instance = this;

        if (item != null)
            item.SetActive(false);
    }

    void Start()
    {
        dialogueTable = DialogueLoader.LoadCSV();
        Name.gameObject.SetActive(false);
        Dialogue.gameObject.SetActive(false);

        StartEvent(0);
    }

    void Update()
    {
        if (ProblemManager.Instance != null &&
            ProblemManager.Instance.IsProblemOpen)
        {
            return;
        }

        if (ChoiceManager.Instance != null && ChoiceManager.Instance.choicePanel.activeSelf)
        {
            return;
        }

        if ((playerPanel.activeSelf || npcPanel.activeSelf) &&
            (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            NextLine();
        }
    }

    public void StartEvent(int eventIndex)
    {

        if (!dialogueTable.ContainsKey(eventIndex))
        {
            return;
        }

        IsDialoguePlaying = true;

        currentEvent = eventIndex;
        if (eventIndex == 13 && event13Played)
        {
            currentLine = dialogueTable[eventIndex].Count -1;
            Name.gameObject.SetActive(true);
            Dialogue.gameObject.SetActive(true);
            ShowCurrentLine();
            ChoiceManager.Instance.OpenChoice();
            return;
        }


        currentLine = 0;

        Name.gameObject.SetActive(true);
        Dialogue.gameObject.SetActive(true);

        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        DialogueLine line = dialogueTable[currentEvent][currentLine];

        bool isPlayer = line.speakerName == "수룡이";
        playerPanel.SetActive(isPlayer);
        npcPanel.SetActive(!isPlayer);

        Name.text = line.speakerName;
        Dialogue.text = line.dialogue;

        if (currentEvent == 7 && currentLine == 0)
        {
            problem1Explanagtion.SetActive(true);
            currentExplanation = problem1Explanagtion;
            showingExplanation = true;
        }

        if (currentEvent == 9 && currentLine == 0)
        {
            problem2Explanation.SetActive(true);
            currentExplanation = problem2Explanation;
            showingExplanation = true;
        }

        // 이전 초상화 숨기기
        if (currentPortrait != null)
        {
            currentPortrait.SetActive(false);
        }

        // Speaker 이름으로 초상화 가져오기
        currentPortrait = PortraitManager.Instance.GetPortrait(line.speakerName);

        if (currentPortrait != null)
        {
            currentPortrait.SetActive(true);
        }

        waitingForEndAction = currentLine == dialogueTable[currentEvent].Count -1
        && !string.IsNullOrEmpty(line.endAction);
    }

    void NextLine()
    {
        if (showingExplanation)
        {
            currentExplanation.SetActive(false);
            showingExplanation = false;
            
            currentLine++;
            ShowCurrentLine();
            return;
        }

        if (waitingForEndAction)
        {
            waitingForEndAction = false;

            DialogueLine line = dialogueTable[currentEvent][currentLine];

            ExecuteEndAction(line.endAction);

            return;
        }

        currentLine++;

        if (currentLine >= dialogueTable[currentEvent].Count)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();
    }

    public void EndDialogue()
    {
        Name.gameObject.SetActive(false);
        Dialogue.gameObject.SetActive(false);
        playerPanel.SetActive(false);
        npcPanel.SetActive(false);

        Name.text = "";
        Dialogue.text = "";

        if (currentPortrait != null)
        {
            currentPortrait.SetActive(false);
            currentPortrait = null;
        }

        IsDialoguePlaying = false;

        if (currentEvent == 17)
        {
            if (item != null)
            {
                item.SetActive(true);
            }
        }
        

        DialogueLine lastLine = dialogueTable[currentEvent][dialogueTable[currentEvent].Count -1];
        if (lastLine.nextStoryStep >=0 )
        {
            GameManager.Instance.storyStep = lastLine.nextStoryStep;

            if (lastLine.nextStoryStep == 2)
            {
                GameManager.Instance.LocationPoint.SetActive(true);
            }
        }

    }

    void ExecuteEndAction(string action)
    {
        
        switch(action)
        {
            case "OpenProblem1":
                ProblemManager.Instance.OpenProblem1();
                break;

            case "RetryProblem1":
                ProblemManager.Instance.OpenProblem1();
                break;

            case "OpenProblem2":
                ProblemManager.Instance.OpenProblem2();
                break;

            case "RetryProblem2":
                ProblemManager.Instance.OpenProblem2();
                break;

            case "OpenChoice":
                if (currentEvent == 13)
                {
                    event13Played = true;
                }
                ChoiceManager.Instance.OpenChoice();
                break;

            case "ExitQuizRoom":
                EndDialogue();
                TwoChoiceRoomManager.Instance.ExitQuizRoom();
                break;
            
            case "ChangeNPCState":
                SecondStateNPC.ChangeToSecondState();
                PortraitManager.Instance.ChangeComputerToNormal();
                EndDialogue();
                StartEvent(17);
                break;


        }
    }
}