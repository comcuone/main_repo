using UnityEngine;
using UnityEngine.UI;

public class Sungshin_BigProblem : MonoBehaviour
{
    public Sungshin_QuestManager questManager;
    public Sungshin_ProblemManager problemManager;

    [Header("선택지 (A, B, C, D 순서)")]
    public Image[] choiceImage; //선택지 이미지 저장

    // 현재 선택 위치
    // 0 = A
    // 1 = B
    // 2 = C
    // 3 = D
    private int selectIndex;

    // 처음 켜질 때 Space 입력 방지
    private bool canInput;

    void OnEnable() //문제가 켜질 때 선택상태 초기화
    {
        selectIndex = 0;

        canInput = false;

        UpdateChoiceColor(); //선택된 보기의 색상 갱신
    }

    void Update()
    {
        if (!canInput)
        {
            if (!Input.GetKey(KeyCode.Space))
                canInput = true;

            return;
        }

        MoveChoice();
        SelectAnswer();
    }

    void MoveChoice() //방향키로 선택지 이동
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // ←
        {
            switch (selectIndex)
            {
                case 0: selectIndex = 1; break; //현재 0번 보기를 선택중이라면 1번으로 이동한다
                case 1: selectIndex = 0; break;
                case 2: selectIndex = 3; break;
                case 3: selectIndex = 2; break;
            }

            UpdateChoiceColor();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) // →
        {
            switch (selectIndex)
            {
                case 0: selectIndex = 1; break;
                case 1: selectIndex = 0; break;
                case 2: selectIndex = 3; break;
                case 3: selectIndex = 2; break;
            }

            UpdateChoiceColor();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) //↑
        {
            switch (selectIndex)
            {
                case 0: selectIndex = 2; break;
                case 1: selectIndex = 3; break;
                case 2: selectIndex = 0; break;
                case 3: selectIndex = 1; break;
            }

            UpdateChoiceColor();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) //↓
        {
            switch (selectIndex)
            {
                case 0: selectIndex = 2; break;
                case 1: selectIndex = 3; break;
                case 2: selectIndex = 0; break;
                case 3: selectIndex = 1; break;
            }

            UpdateChoiceColor();
        }
    }

    void UpdateChoiceColor() //현재 선택된 보기만 밝게 표시하고 나머지는 어둡게 표시
    {
        for (int i = 0; i < choiceImage.Length; i++) //모든 선택지를 순서대로 확인
        {
            if (i == selectIndex)
            {
                choiceImage[i].color = Color.white; //현재 선택된 보기를 흰색으로 표시
            }
            else
            {
                choiceImage[i].color = new Color(0.75f, 0.75f, 0.75f, 1f); //선택되지 않은 보기를 회색으로 표시
            }
        }
    }

    void SelectAnswer()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) //사용자가 스페이스바를 눌렀는지 검사
            return; 

        // 정답 : C(FAT)
        if (selectIndex == 2)
        {
            Success();
        }
        else
        {
            Fail();
        }
    }

    void Success()
    {
        questManager.npc3000Clear = true; //Big problem 성공

        problemManager.CloseBigProblem(); //Problem UI 닫기

        //Sungshin_DialogueManager를 찾아 변수에 저장한 뒤, 대화를 시작하는 StartDialogue() 함수를 호출하기 위해 사용
        Sungshin_DialogueManager dialogueManager =
            FindFirstObjectByType<Sungshin_DialogueManager>(); 

        dialogueManager.StartDialogue(3110); //성공 대사 출력
    }

    void Fail()
    {
        problemManager.CloseBigProblem();

        Sungshin_DialogueManager dialogueManager =
            FindFirstObjectByType<Sungshin_DialogueManager>();

        dialogueManager.StartDialogue(3120); //오답 대사 출력
    }
}