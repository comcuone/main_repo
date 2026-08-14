using UnityEngine;
using UnityEngine.UI;

public class Sungshin_SmallProblem : MonoBehaviour
{
    public Sungshin_QuestManager questManager;
    public Sungshin_ProblemManager problemManager;

    [Header("선택 표시")]
    public RectTransform choice;
    public float[] choicePosX;

    [Header("위 캐릭터")]
    public Image[] characterImage;

    [Header("답안칸")]
    public Image[] answerImage;

    [Header("빈 사람")]
    public Sprite emptySprite;

    private int selectIndex;
    private int answerIndex;
    private bool[] used = new bool[5];
    private int[] playerAnswer = new int[5];
    private int[] correctAnswer = { 3, 2, 1, 0, 4 };

    // 처음 한 번은 스페이스를 눌러야 조작 시작
    private bool canInput;

    void OnEnable()
    {
        ResetProblem();
        canInput = false;
    }

    void Update()
    {
        // 처음 스페이스는 버림
        if (!canInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canInput = true;
            }

            return;
        }

        MoveChoice();
        SelectCharacter();
    }

    void MoveChoice()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectIndex > 0)
                selectIndex--;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectIndex < 4)
                selectIndex++;
        }

        Vector3 pos = choice.localPosition;
        pos.x = choicePosX[selectIndex];
        choice.localPosition = pos;
    }

    void SelectCharacter()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (used[selectIndex])
            return;

        if (answerIndex >= answerImage.Length)
            return;

        answerImage[answerIndex].sprite = characterImage[selectIndex].sprite;
        playerAnswer[answerIndex] = selectIndex;
        used[selectIndex] = true;

        characterImage[selectIndex].color =
            new Color(0.5f, 0.5f, 0.5f, 1f);

        answerIndex++;

        if (answerIndex >= answerImage.Length)
        {
            CheckAnswer();
        }
    }

    void CheckAnswer()
    {
        for (int i = 0; i < correctAnswer.Length; i++)
        {
            if (playerAnswer[i] != correctAnswer[i])
            {
                Fail();
                return;
            }
        }

        Success();
    }

    void Success()
    {
        questManager.npc2000Clear = true;

        problemManager.CloseSmallProblem();

        Sungshin_DialogueManager dialogueManager =
            FindFirstObjectByType<Sungshin_DialogueManager>();

        dialogueManager.StartDialogue(2110);
    }

    void Fail()
    {
        problemManager.CloseSmallProblem();

        Sungshin_DialogueManager dialogueManager =
            FindFirstObjectByType<Sungshin_DialogueManager>();

        dialogueManager.StartDialogue(2120);
    }

    void ResetProblem()
    {
        selectIndex = 0;
        answerIndex = 0;

        Vector3 pos = choice.localPosition;
        pos.x = choicePosX[0];
        choice.localPosition = pos;

        for (int i = 0; i < answerImage.Length; i++)
        {
            answerImage[i].sprite = emptySprite;
            playerAnswer[i] = -1;
            used[i] = false;
        }

        for (int i = 0; i < characterImage.Length; i++)
        {
            characterImage[i].color = Color.white;
        }
    }
}