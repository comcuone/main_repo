using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class nan_QuizManager : MonoBehaviour
{
    public static nan_QuizManager Instance;

    [Header("Loader")]
    public nan_CSVQuizLoader loader;

    [Header("Quiz UI")]
    public GameObject quizPanel;
    public GameObject wrongPanel;

    public TMP_Text questionText;

    public TMP_Text answer1Text;
    public TMP_Text answer2Text;
    public TMP_Text answer3Text;
    public TMP_Text answer4Text;

    public GameObject select1;
    public GameObject select2;
    public GameObject select3;
    public GameObject select4;

    private nan_QuizNode currentQuiz;

    private int currentQuizID;

    private int selected = 0;

    private bool quizOpen = false;

    public bool IsQuizOpen => quizOpen;

    private bool wrongOpen = false;

    void Awake()
    {
        Instance = this;

        quizPanel.SetActive(false);
        wrongPanel.SetActive(false);

        HideSelect();
    }

    void Update()
    {
        if (!quizOpen)
            return;

        if (wrongOpen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                wrongPanel.SetActive(false);
                wrongOpen = false;
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selected >= 2)
                selected -= 2;

            UpdateSelect();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selected <= 1)
                selected += 2;

            UpdateSelect();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selected % 2 == 1)
                selected--;

            UpdateSelect();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selected % 2 == 0)
                selected++;

            UpdateSelect();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckAnswer();
        }
    }

    public void StartQuiz(int quizID)
    {
        currentQuizID = quizID;

        currentQuiz = loader.database.GetQuiz(quizID);

        if (currentQuiz == null)
        {
            Debug.LogError("퀴즈를 찾을 수 없습니다.");
            return;
        }

        quizOpen = true;

        quizPanel.SetActive(true);

        wrongPanel.SetActive(false);

        questionText.text = currentQuiz.Question;

        answer1Text.text = currentQuiz.Choice1;
        answer2Text.text = currentQuiz.Choice2;
        answer3Text.text = currentQuiz.Choice3;
        answer4Text.text = currentQuiz.Choice4;

        selected = 0;

        UpdateSelect();
    }

    void HideSelect()
    {
        select1.SetActive(false);
        select2.SetActive(false);
        select3.SetActive(false);
        select4.SetActive(false);
    }

    void UpdateSelect()
    {
        HideSelect();

        switch (selected)
        {
            case 0:
                select1.SetActive(true);
                break;

            case 1:
                select2.SetActive(true);
                break;

            case 2:
                select3.SetActive(true);
                break;

            case 3:
                select4.SetActive(true);
                break;
        }
    }
        void CheckAnswer()
    {
        // Answer는 1~4로 저장되어 있으므로
        if (selected + 1 == currentQuiz.Answer)
        {
            NextQuiz();
        }
        else
        {
            wrongOpen = true;
            wrongPanel.SetActive(true);
        }
    }

    void NextQuiz()
    {
        quizPanel.SetActive(false);
        wrongPanel.SetActive(false);

        quizOpen = false;
        wrongOpen = false;

        HideSelect();

        nan_NPCInteraction npc = nan_DialogueManager.Instance.GetCurrentNPC();

        if (npc != null)
        {
            npc.NextStage();
        }

        nan_DialogueNode node = nan_DialogueManager.Instance.GetCurrentNode();

        if (node == null)
            return;

        nan_DialogueManager.Instance.SetCurrentNode(node.Next);
    }

    public void CloseQuiz()
    {
        quizPanel.SetActive(false);
        wrongPanel.SetActive(false);

        quizOpen = false;
        wrongOpen = false;

        HideSelect();
    }
}