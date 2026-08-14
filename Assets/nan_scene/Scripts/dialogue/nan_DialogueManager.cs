using UnityEngine;
using TMPro;

public class nan_DialogueManager : MonoBehaviour
{
    public static nan_DialogueManager Instance;

    [Header("Dialogue UI")]
    public GameObject dimBackground;

    public GameObject hungryPanel;
    public TMP_Text hungryName;
    public TMP_Text hungryText;

    public GameObject quizPanel;
    public TMP_Text quizName;
    public TMP_Text quizText;

    public GameObject playerPanel;
    public TMP_Text playerName;
    public TMP_Text playerText;

    public GameObject choicePanel;
    public TMP_Text choice1Text;
    public TMP_Text choice2Text;
    public RectTransform arrow;

    private bool isChoosing = false;
    private int selectedChoice = 0;

    public GameObject minigameBackground;
    private nan_CSVDialogueLoader loader;
    private nan_DialogueNode currentNode;
    private nan_NPCInteraction currentNPC;

    public bool IsOpen { get; private set; }

    public bool LastDialogueFinished = false;

    private void Awake()
    {

        Instance = this;

        loader = GetComponent<nan_CSVDialogueLoader>();

        CloseAllUI();
    }

    private void Update()
    {
        if (nan_QuizManager.Instance != null && nan_QuizManager.Instance.IsQuizOpen)
            return;
        if (!IsOpen)
            return;

        if (isChoosing)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedChoice = 0;
                UpdateChoiceArrow();
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedChoice = 1;
                UpdateChoiceArrow();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SelectChoice();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }
    }

    public void StartDialogue(int startID)
    {

        if (loader == null)
        {
            Debug.LogError("nan_CSVDialogueLoader가 없습니다!");
            return;
        }

        currentNode = loader.database.GetNode(startID);

        if (currentNode == null)
        {
            Debug.LogError("ID " + startID + " 를 찾을 수 없습니다.");
            return;
        }

        IsOpen = true;

        ShowDialogue();
    }

    void ShowDialogue()
    {
        CloseAllUI();

        if (currentNode.QuizID != -1)
        {
            nan_QuizManager.Instance.StartQuiz(currentNode.QuizID);
            return;
        }
        
        if (currentNode.Type.ToLower() == "minigame")
        {
            StartMinigame();
            return;
        }

        dimBackground.SetActive(true);

        switch (currentNode.Portrait.ToLower())
        {
            case "hungry":
                hungryPanel.SetActive(true);
                hungryName.text = currentNode.Speaker;
                hungryText.text = currentNode.Text;
                break;

            case "quiz":
                quizPanel.SetActive(true);
                quizName.text = currentNode.Speaker;
                quizText.text = currentNode.Text;
                break;

            case "player":
                playerPanel.SetActive(true);
                playerName.text = currentNode.Speaker;
                playerText.text = currentNode.Text;
                break;
        }

        if (currentNode.Type.ToLower() == "choice")
        {
            choicePanel.SetActive(true);

            choice1Text.text = currentNode.Choice1;
            choice2Text.text = currentNode.Choice2;

            selectedChoice = 0;
            isChoosing = true;

            UpdateChoiceArrow();
        }
        else
        {
            isChoosing = false;
        }
    }

    public void NextDialogue()
    {
        if (isChoosing)
            return;

        if (currentNode.Next == -1)
        {
            LastDialogueFinished = true;

            if (currentNPC != null && currentNPC.ShouldFinish())
            {
                currentNPC.FinishInteraction();
            }

            CloseDialogue();
            return;
        }

        currentNode = loader.database.GetNode(currentNode.Next);

        if (currentNode == null)
        {
            CloseDialogue();
            return;
        }

        ShowDialogue();
    }

    public void CloseDialogue()
    {
        IsOpen = false;
        CloseAllUI();
        isChoosing = false;
    }

    public void StartMinigame()
    {
        IsOpen = false;
        isChoosing = false;

        CloseAllUI();

        minigameBackground.SetActive(true);
    }

    void CloseAllUI()
    {
        dimBackground.SetActive(false);

        hungryPanel.SetActive(false);
        quizPanel.SetActive(false);
        playerPanel.SetActive(false);

        choicePanel.SetActive(false);
    }

    void UpdateChoiceArrow()
    {
        Vector3 pos = arrow.position;

        if (selectedChoice == 0)
            pos.y = choice1Text.transform.position.y;
        else
            pos.y = choice2Text.transform.position.y;

        arrow.position = pos;
    }

    void SelectChoice()
    {
        int nextID;

        if (selectedChoice == 0)
            nextID = currentNode.Choice1Next;
        else
            nextID = currentNode.Choice2Next;

        isChoosing = false;

        currentNode = loader.database.GetNode(nextID);

        if (currentNode == null)
        {
            CloseDialogue();
            return;
        }

        ShowDialogue();
    }

    public nan_DialogueNode GetCurrentNode()
    {
        return currentNode;
    }

    public void SetCurrentNode(int id)
    {
        currentNode = loader.database.GetNode(id);

        if (currentNode == null)
        {
            CloseDialogue();
            return;
        }

        ShowDialogue();
    }

    public void SetCurrentNPC(nan_NPCInteraction npc)
    {
        currentNPC = npc;
    }

    public nan_NPCInteraction GetCurrentNPC()
    {
        return currentNPC;
    }

    public void ResumeDialogue()
    {
        IsOpen = true;

        NextDialogue();
    }
}