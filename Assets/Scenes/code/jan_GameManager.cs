using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems; // EventSystem 사용을 위해 추가
using UnityEngine.UI;
using TMPro;


public class jan_GameManager : MonoBehaviour
{

    [Header("0. 웹툰 연출 시스템")]
    public GameObject webtoonPanel;      // 웹툰 전체 캔버스/패널
    public GameObject[] webtoonCuts;    // 웹툰 컷 4개의 GameObject 배열
    private int currentCutIndex = 0;    // 현재 켜진 컷 번호
    private bool isWebtoonActive = false; // 웹툰 연출 중인지 여부 (이동 및 입력 제어용)
    private bool isAnimating = false;    // 애니메이션 진행 중 여부 (중복 클릭 방지)

    // PlayerController 등 외부 이동 스크립트에서 참조할 프로퍼티
    public bool IsWebtoonActive => isWebtoonActive;


    [Header("웹툰 연출 옵션")]
    [Tooltip("아래에서 올라올 거리")]
    public float startOffsetY = 200f; 
    [Tooltip("컷이 올라오며 나타나는 시간 (초)")]
    public float animateDuration = 0.5f;

    private Vector2[] originalCutPositions;

    [Header("고양이 말풍선 UI")]
    public GameObject catSpeechBubbleUI;        // 고양이 머리 위에 위치한 말풍선 GameObject/Canvas

    [Header("대화")]
    public int talkIndex;
    public jan_TalkManager talkManager;
    public GameObject scanObject;
    public GameObject talkPanel;
    public GameObject talkBackground;
    public RectTransform character;

    [Header("대화 UI")]
    public Image dialoguePanelImage;     
    public Sprite suryongBubble;         
    public Sprite catBubble;             
    public TextMeshProUGUI speakerText;  
    public TextMeshProUGUI talkText;     
    public bool isAction;
    public Sprite suryongImage;
    public Sprite catImage;
    public Image characterImage;

    [Header("퀴즈 도전 여부 선택창 (예/아니오)")]
    public GameObject acceptPanel;       
    public Button yesButton;            
    public Button noButton;      
    public GameObject yesPoint;
    public GameObject noPoint;       

    [Header("퀴즈 패널 및 4개 선택지")]
    public GameObject quizPanel;        
    public Button[] choiceButtons;   


    // 상태 제어 변수
    private bool isChoiceActive;        
    private bool isQuizActive;          
    private bool isQuestCleared;        
    private int currentNpcId;           
    
    // 선택창 방향키 조작을 위한 변수 (0: 예, 1: 아니오)
    private int currentAcceptChoice = 0;

    void Start()
    {
        // 1. 초기 UI 및 말풍선 비활성화
        talkPanel.SetActive(false);
        if (acceptPanel != null) acceptPanel.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(false);
        if (catSpeechBubbleUI != null) catSpeechBubbleUI.SetActive(false);


        // 도전 여부 버튼 이벤트 등록, 버튼이 눌리면 함수 실행
        if (yesButton != null) yesButton.onClick.AddListener(OnAcceptQuiz);
        if (noButton != null) noButton.onClick.AddListener(OnRefuseQuiz);

        // 퀴즈 선택지 버튼 이벤트 등록
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            choiceButtons[i].onClick.AddListener(() => OnSelectChoice(index));
        }
        if (characterImage != null) characterImage.enabled = false;

        // 2. 웹툰 연출 시작
        StartWebtoon();
    }

    void Update()
    {
        // 웹툰 연출 진행 중일 때만 스페이스바/마우스 클릭 받기
        if (isWebtoonActive)
        {
            if (isAnimating) return;//페이드인 연출 중일 때에는 넘어가지 않음

            if (currentCutIndex < webtoonCuts.Length)// 다음 웹툰이 남아 있는 경우 스페이스 바 또는 좌클릭을 하면 넘어감
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    AdvanceWebtoon(); 
                }
            }
            return;
        }
        
        // 도전 여부 선택창 방향키 및 엔터 조작 로직
        if (isChoiceActive && acceptPanel != null && acceptPanel.activeSelf)
        {
            //선택창이 활성화되어있는 경우 좌/우 (또는 상/하) 방향키로 선택지 이동
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || 
                Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                // 0과 1을 토글
                currentAcceptChoice = (currentAcceptChoice == 0) ? 1 : 0;
                UpdateAcceptPointers();
            }

            // 엔터키 또는 스페이스바를 누르면 선택 실행
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                if (currentAcceptChoice == 0)
                    OnAcceptQuiz();
                else
                    OnRefuseQuiz();
            }
            return; // 선택창이 켜져있을 때는 일반 대화 넘김 방지
        }

        // 대화 중일 때 스페이스바를 누르면 다음 대화로 진행
        if (isAction && !isChoiceActive && !isQuizActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Talk(currentNpcId);
            }
        }
    }

    // --- [0] 웹툰 연출 및 부드러운 등장 로직 ---

    void StartWebtoon()
    {
        if (webtoonPanel != null && webtoonCuts.Length > 0)
        {// 현재 상태를 웹툰 재생 중으로 바꾸고 UI 패널을 보이게 함
            isWebtoonActive = true;
            webtoonPanel.SetActive(true);

            originalCutPositions = new Vector2[webtoonCuts.Length];// 애니메이션을 위해 원래 위치를 저장할 변수 설정

            for (int i = 0; i < webtoonCuts.Length; i++) // 웹툰의 컷 개수만큼 반복
            {
                if (webtoonCuts[i] != null)
                {
                    RectTransform rect = webtoonCuts[i].GetComponent<RectTransform>();
                    originalCutPositions[i] = rect.anchoredPosition;
                    webtoonCuts[i].SetActive(false);
                } // 컷의 위치를 저장하고 화면에서 숨김
            }

            currentCutIndex = 0;//인덱스 초기화
            StartCoroutine(AnimateCut(currentCutIndex));
        }
        else
        {
            isWebtoonActive = false;
        }
    }

    void AdvanceWebtoon()
    {
        currentCutIndex++;

        if (currentCutIndex < webtoonCuts.Length)//남은 컷이 있다면 다음 컷을 띄우는 함수 실행
        {
            StartCoroutine(AnimateCut(currentCutIndex));
        }
        else
        {
            EndWebtoon();//종료 함수 실행
        }
    }

    IEnumerator AnimateCut(int index)
    {
        isAnimating = true;

        GameObject cutObj = webtoonCuts[index];
        if (cutObj == null)
        {
            isAnimating = false;
            yield break;
        }

        RectTransform rect = cutObj.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = cutObj.GetComponent<CanvasGroup>();//위치와 투명도 가져오기

        if (canvasGroup == null)
        {
            canvasGroup = cutObj.AddComponent<CanvasGroup>();
        }

        Vector2 targetPos = originalCutPositions[index]; // 컷의 최종 위치
        Vector2 startPos = targetPos - new Vector2(0, startOffsetY);// 컷의 시작위치

        rect.anchoredPosition = startPos; 
        canvasGroup.alpha = 0f;
        cutObj.SetActive(true);//투명도를 0으로 만들고 컷을 시작 위치로 옮김, 컷을 보이도록 함

        float elapsedTime = 0f;

        while (elapsedTime < animateDuration)
        {
            elapsedTime += Time.deltaTime;// 이전 프레임부터 지금까지 걸린 시간을 더함
            float t = elapsedTime / animateDuration; // 애니메이션 진행도
            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f); // 올라오던 속도 연출

            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, smoothT);// 시작지점에서 목표지점까지 t비율만큼 이동
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);// 투명도 조절

            yield return null;
        }
// 위치와 투명도를 최종적으로 수정함
        rect.anchoredPosition = targetPos;
        canvasGroup.alpha = 1f;
// 애니메이션을 끝냄
        isAnimating = false;
    }

    void EndWebtoon()
    {
        // 1. 웹툰 패널 비활성화
        if (webtoonPanel != null) 
        {
            webtoonPanel.SetActive(false);
        }

        // 2. 웹툰 상태 종료 (플레이어 이동 제어 해제)
        isWebtoonActive = false;

        // 3. 바로 수룡이 첫 대화(ID 1500) 시작
        currentNpcId = 1500;
        talkIndex = 0;
        // 대화창 패널, 일러스트를 보이도록 하고 대화 텍스트를 출력함
        talkPanel.SetActive(true);
        if (characterImage != null) characterImage.enabled = true;
        
        Talk(currentNpcId);
    }

    // --- [1] NPC 상호작용 및 대화 로직 ---

    public void Action(GameObject scanObj)
    {
        if (isWebtoonActive || isChoiceActive || isQuizActive) return;//웹툰 또는 선택창 또는 퀴즈창일 경우 작동하지 않음

        if (isAction) return;

        // 고양이와 대화를 시작하는 순간 고양이 머리 위 말풍선 끄기
        if (catSpeechBubbleUI != null)
        {
            catSpeechBubbleUI.SetActive(false);
        }

        scanObject = scanObj;// 상호작용 대상 저장하고 ID를 받아옴
        jan_ObjectData objectData = scanObject.GetComponent<jan_ObjectData>();
        
        if (isQuestCleared && objectData.id == 1000)
        {
            currentNpcId = 1400; 
        }// 상호작용 후 다시 말을 걸면 ID를 1400으로 변경
        else
        {
            currentNpcId = objectData.id;
        }// ID 실행

        talkIndex = 0; // 대화순서를 첫 문장으로 초기화
        Talk(currentNpcId);
        talkPanel.SetActive(true);//대화창 패널과 배경, 일러스트레이터를 보이도록 함
        talkBackground.SetActive(true);
        if (characterImage != null) characterImage.enabled = true;
    }

    void Talk(int id)
    {
        var talkData = talkManager.GetTalk(id, talkIndex);// talkManager를 통해 ID와 대화 순서에 맞는 speaker과 대사를 받음

        // 남은 대사가 없을 경우 대화창, 패널, 일러스트 등을 안 보이도록 변경함
        if (talkData == null)
        {
            isAction = false;
            talkPanel.SetActive(false);
            talkBackground.SetActive(false);
            if (characterImage != null) characterImage.enabled = false;
        
            if (acceptPanel != null) acceptPanel.SetActive(false);
            if (quizPanel != null) quizPanel.SetActive(false);

            // ID 1500번 수룡이 대화가 완전히 끝난 직후 고양이 위에 말풍선 띄우기
            if (id == 1500)
            {
                if (catSpeechBubbleUI != null)
                {
                    catSpeechBubbleUI.SetActive(true);
                }
            }

            talkIndex = 0;
            return;
        }
// 발화자와 대화 내용을 저장할 변수 설정
        string currentSpeaker = talkData.Value.speaker;
        string currentTalk = talkData.Value.talk;

        UpdateDialogueBubble(currentSpeaker);// 발화자에 따라 말풍선, 위치 등을 전환하는 함수

        if (speakerText != null) speakerText.text = currentSpeaker;
        talkText.text = currentTalk;
        isAction = true;
// ID 가 1000이고 talkIndex 가 3이면 선택지창을 보여줌
        if (id == 1000 && talkIndex == 3)
        {
            ShowAcceptPanel(true);
        }

        talkIndex++;
    }

    void UpdateDialogueBubble(string speaker)
    {
        if (dialoguePanelImage == null) return;
        RectTransform rectTransform = speakerText.GetComponent<RectTransform>();//speaker이 적힌 텍스트 위치 가져옴
        if (speaker == "수룡이")// 만약 speaker가 수룡이라면 말풍선과 일러스트를 수룡이 전용으로 전환하고, 위치를 옮김
        {
            if (suryongBubble != null) {
                dialoguePanelImage.sprite = suryongBubble;
                if (characterImage != null) characterImage.sprite = suryongImage;
                rectTransform.anchoredPosition = new Vector2 (32.3f, -135f);
                character.anchoredPosition = new Vector2(-793f,-306f);
            }
        }
        else 
        {// 수룡이가 아닐 경우 고양이의 말풍선과 일러스트로 전환, 
            if (catBubble != null) {
                dialoguePanelImage.sprite = catBubble;
                if (characterImage != null) characterImage.sprite = catImage;
                rectTransform.anchoredPosition = new Vector2 (86.4f, -135f);
                character.anchoredPosition = new Vector2(743f,-306f);

            }
        }
    }

    // --- [2] 도전 선택창 로직 ---

    void ShowAcceptPanel(bool isShow)
    {
        isChoiceActive = isShow;
        if (acceptPanel != null) acceptPanel.SetActive(isShow);
        
        //패널이 켜질 때 '예(0)'를 기본값으로 설정하고 포인터 업데이트
        if (isShow)
        {
            currentAcceptChoice = 0;
            UpdateAcceptPointers();
        }
    }

    // 화살표 포인터 및 UI 선택 상태 업데이트
    void UpdateAcceptPointers()
    {
        // 화살표 활성/비활성화 처리
        if (yesPoint != null) yesPoint.SetActive(currentAcceptChoice == 0);//선택지에 포커스가 가면 조건이 참이 되어 화면에서 보임
        if (noPoint != null) noPoint.SetActive(currentAcceptChoice == 1);

        // Unity EventSystem을 통해 버튼 Highlight 시각 효과 동기화
        if (currentAcceptChoice == 0 && yesButton != null)
            EventSystem.current.SetSelectedGameObject(yesButton.gameObject);// 포커스가 될 경우 버튼 색상 변경
        else if (currentAcceptChoice == 1 && noButton != null)
            EventSystem.current.SetSelectedGameObject(noButton.gameObject);
    }

    public void OnAcceptQuiz()
    {
        ShowAcceptPanel(false);// 선택지 창을 안 보이도록 함
//ID 와 인덱스를 세팅함
        currentNpcId = 1100;
        talkIndex = 0;
        //talkManager를 통해 대사를 가져와서 띄움
        var quizQuestion = talkManager.GetTalk(currentNpcId, talkIndex);
        if (quizQuestion != null)
        {
            UpdateDialogueBubble(quizQuestion.Value.speaker);
            if (speakerText != null) speakerText.text = quizQuestion.Value.speaker;
            talkText.text = quizQuestion.Value.talk;
        }

        isQuizActive = true; // 게임 상태를 퀴즈 중으로 전환하여 상호작용 키를 눌러도 작동하지 않음
        if (quizPanel != null) quizPanel.SetActive(true);// 문제 선택지를 보여줌
    }

    public void OnRefuseQuiz() // 선택지 창에서 거절을 선택했을 경우 실행됨
    {// 선택지 창을 안 보이도록 하고 ID와 Index를 세팅하고 함수 실행 
        ShowAcceptPanel(false);

        currentNpcId = 1300;
        talkIndex = 0;
        Talk(currentNpcId);
    }

    // --- [3] 퀴즈 정답/오답 로직 ---

    public void OnSelectChoice(int choiceIndex)
    {
        if (choiceIndex == 0) // 정답 인덱스를 선택한 경우 실행
        {// 현재 상태를 문제를 푸는 중이 아니라고 설정, 퀘스트 완료 상태로 전환
            isQuizActive = false;
            isQuestCleared = true;
            if (quizPanel != null) quizPanel.SetActive(false);
// 문제창이 보이지 않도록 설정, 대화 출력
            currentNpcId = 1200;
            talkIndex = 0;
            Talk(currentNpcId);
        }
        else 
        {// 오답을 고른 경우 대사 출력
            UpdateDialogueBubble("고양이");
            if (speakerText != null) speakerText.text = "고양이";
            talkText.text = "틀렸어 냐옹! 다시 한번 잘 생각해보라구!";
        }
    }
}