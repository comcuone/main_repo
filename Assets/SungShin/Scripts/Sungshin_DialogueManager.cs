using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sungshin_DialogueManager : MonoBehaviour
{
    public static Sungshin_DialogueManager Instance;
    // ===== 선택지 UI =====
    [SerializeField] private GameObject choicePanel;      // 선택지 패널
    [SerializeField] private TextMeshProUGUI choice1;     // 첫 번째 선택지 텍스트
    [SerializeField] private TextMeshProUGUI choice2;     // 두 번째 선택지 텍스트
    [SerializeField] private RectTransform arrow;         // 현재 선택 중인 항목을 가리키는 화살표

    [SerializeField] private float choice1Y;              // 첫 번째 선택지의 화살표 Y좌표
    [SerializeField] private float choice2Y;              // 두 번째 선택지의 화살표 Y좌표

    // ===== 다른 매니저 참조 =====
    public Sungshin_ProblemManager problemManager;        // 문제(퀴즈) UI 관리
    public Sungshin_QuestManager questManager;            // 퀘스트 진행 상태 관리
    public Sungshin_TextManager textManager;              // 대사 데이터 관리

    // ===== 대화창 UI =====
    [SerializeField]
    public GameObject playerPanel;                        // 플레이어 대사창

    [SerializeField]
    public GameObject npcPanel;                           // NPC 대사창

    [SerializeField]
    public GameObject background;                         // 대화창 배경

    // ===== 대화 상태 =====
    public bool isAction;                                // 현재 대화가 진행 중인지 여부
    public GameObject CObject;                           // 현재 대화 중인 NPC 오브젝트

    // ===== 대사 출력 =====
    public TextMeshProUGUI npcText;                      // NPC 대사 텍스트
    public TextMeshProUGUI playerText;                   // 플레이어 대사 텍스트
    public TextMeshProUGUI Name;                         // NPC 이름

    [SerializeField]
    private Image Portrait;                              // NPC 초상화 이미지

    private Sungshin_Npc npc;                            // 현재 대화 중인 NPC 정보

    public int textIndex;                                // 현재 출력 중인 대사의 순서

    // ===== 선택지 정보 =====
    private bool isChoice = false;                       // 현재 선택지가 있는 대사인지 여부
    private int choiceIndex = 0;                         // 현재 선택된 선택지 번호

    private Sungshin_Dialogue currentDialogue;           // 현재 출력 중인 대사
    public int currentID;                               // 현재 진행 중인 대사 ID

    // ===== 정전 이벤트 =====
    [SerializeField]
    private GameObject blackPanel;                       // 화면을 검게 덮는 UI

    [SerializeField]
    private Transform npc4000;                           // 이동시킬 NPC

    [SerializeField]
    private Vector2 npc4000MovePos;                      // NPC가 이동할 위치

    public bool isEvent = false;                         // 이벤트 진행 여부

    // ===== 마지막 문제 관련 =====
    [SerializeField] private GameObject lastProblemUI;   // 마지막 문제 UI

    // ===== 정전 이후 등장/비활성화되는 NPC =====
    [SerializeField] private GameObject npc2000Object;
    [SerializeField] private GameObject npc3000Object;
    [SerializeField] private GameObject npc4000Object;

    [SerializeField] private GameObject Item;            // 이벤트 완료 후 등장하는 아이템

    private void Awake()
    {
        Instance = this;
    }

    public void Action(GameObject scanObj)
    {
        isAction = true;                     // 대화 시작 상태로 변경
        background.SetActive(true);          // 대화창 배경 표시
        CObject = scanObj;                   // 현재 대화 중인 NPC 저장

        npc = CObject.GetComponent<Sungshin_Npc>();   // NPC 정보 가져오기

        Portrait.sprite = npc.Portrait;      // NPC 초상화 표시
        Name.text = CObject.name;            // NPC 이름 표시

        // 선택지를 누른 뒤 이어지는 대화라면
        if (isChoice)
        {
            // 선택한 선택지의 다음 대사 ID로 이동
            currentID = currentDialogue.choices[choiceIndex].nextID;

            // 퀘스트 시작
            if (currentID == 1001)
            {
                questManager.questStarted = true;
            }

            textIndex = 0;   // 새 대사의 첫 줄부터 시작
        }
        // 처음 NPC와 대화를 시작한 경우
        else if (textIndex == 0)
        {
            // 현재 퀘스트 진행도에 맞는 대사 ID 계산
            int questTextIndex = questManager.GetQuestTextIndex(npc.id);
            currentID = npc.id + questTextIndex;
        }

        Talk(currentID);     // 대사 출력
    }

    void Talk(int id)
    {
        background.SetActive(true);
        currentDialogue = textManager.GetText(id, textIndex); //출력할 대사 가져오기

        if (currentDialogue == null)
        {
           if (currentID == 1200) //정전 이벤트 시작
            {
                isEvent = true;

                isChoice = false;
                textIndex = 0;

                background.SetActive(false);
                npcPanel.SetActive(false);
                playerPanel.SetActive(false);
                choicePanel.SetActive(false);
                arrow.gameObject.SetActive(false);

                StartCoroutine(BlackoutEvent());
                return;
            }

            if (currentID == 1300) //정전 직후
            {
                questManager.blackoutStage = 1;
                npc4000.position = npc4000MovePos;
            }

            isAction = false;
            isChoice = false;
            textIndex = 0;

            background.SetActive(false);
            npcPanel.SetActive(false);
            playerPanel.SetActive(false);
            choicePanel.SetActive(false);
            arrow.gameObject.SetActive(false);

            if (currentID == 2100)
                problemManager.OpenProblem(2000); //Small problem

            if (currentID == 3100)
                problemManager.OpenProblem(3000); //Big problem

            if (currentID == 4401)
            {
                problemManager.isProblem = true;
                lastProblemUI.SetActive(true); //정전 problem
            }

            if (currentID == 1500) //컴공수룡이 퀘스트 완료 + 아이템 지급
            {
                CObject.GetComponent<Objects>().ChangeClothes(); //외형 변경
                CObject.GetComponent<Sungshin_Npc>().ChangePortrait(); //초상화 변경
                
                //컴공수룡이를 제외한 모든 NPC 비활성화
                npc2000Object.SetActive(false);
                npc3000Object.SetActive(false);
                npc4000Object.SetActive(false);
                questManager.blackoutStage = 3;
                Item.SetActive(true);

            }

            return;
        }

        if (currentDialogue.speaker == Speaker.NPC) //NPC 대화창 열기
        {
            npcPanel.SetActive(true);
            playerPanel.SetActive(false);

            npcText.text = currentDialogue.text;
        }
        else //Player 대화창 열기
        {
            npcPanel.SetActive(false);
            playerPanel.SetActive(true);

            playerText.text = currentDialogue.text;
        }

        textIndex++;

        if (currentDialogue.choices != null) //선택지가 있는 대사일 경우
        {
            isChoice = true;
            choiceIndex = 0;

            choicePanel.SetActive(true);

            //선택지 내용 출력
            choice1.text = currentDialogue.choices[0].text;
            choice2.text = currentDialogue.choices[1].text;

            //화살표를 첫 번째 선택지로 이동
            Vector3 pos = arrow.localPosition;
            pos.y = choice1Y;
            arrow.localPosition = pos;

            arrow.gameObject.SetActive(true);
        }
        else //일반 대사일 경우
        {
            isChoice = false;

            choicePanel.SetActive(false);
            arrow.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isChoice) //선택지가 없는 경우 무시한다
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            choiceIndex--;

            //첫 번째 선택지보다 위로 올라가지 못하도록 제한
            if (choiceIndex < 0)
                choiceIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            choiceIndex++;

            //두 번째 선택지보다 아래로 내려가지 못하도록 제한
            if (choiceIndex > 1)
                choiceIndex = 1;
        }

        Vector3 pos = arrow.localPosition; //현재 선택된 항목에 맞게 화살표 위치 변경

        if (choiceIndex == 0)
        {
            pos.y = choice1Y;
        }
        else
        {
            pos.y = choice2Y;
        }

        arrow.localPosition = pos;
    }

    public void StartDialogue(int id)
    {
        isAction = true;          // 대화 시작

        background.SetActive(true); // 대화창 표시

        currentID = id;           // 시작할 대사 ID 저장
        textIndex = 0;            // 첫 번째 대사부터 시작

        Talk(currentID);          // 대사 출력
    }
    IEnumerator BlackoutEvent()
    {
        // 잠시 기다린 뒤
        yield return new WaitForSeconds(0.7f);

        // 화면을 검게 만듦
        blackPanel.SetActive(true);

        // 연출을 위해 잠시 대기
        yield return new WaitForSeconds(1.5f);

        isEvent = false;

        // 정전 이후 대사 시작
        StartDialogue(1300);
    }
}