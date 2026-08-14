using UnityEngine;
using UnityEngine.UI;

public class Sungshin_LastProblem : MonoBehaviour
{
    [SerializeField] private GameObject lastProblemUI;
    // 마지막 퍼즐 UI

    [SerializeField] private GameObject blackPanel;
    // 정전 화면(UI)

    public Sungshin_QuestManager questManager;
    // 퀘스트 진행 상태 관리

    public Sungshin_ProblemManager problemManager;
    // 문제 진행 여부 관리

    [System.Serializable]
    public class Lever
    {
        public GameObject down;   // 레버가 내려간 모습
        public GameObject up;     // 레버가 올라간 모습
    }

    [SerializeField]
    private Lever[] levers = new Lever[8];
    // 8개의 레버 정보 저장

    private int currentLever = 0;
    // 현재 선택된 레버 번호

    private bool[] leverState = new bool[8];
    // 각 레버의 상태(false = 아래, true = 위)

    void OnEnable()
    {
        currentLever = 0;     // 첫 번째 레버 선택

        // 모든 레버를 아래 상태로 초기화
        for (int i = 0; i < levers.Length; i++)
        {
            leverState[i] = false;

            levers[i].down.SetActive(true);
            levers[i].up.SetActive(false);
        }

        UpdateSelection();    // 선택된 레버 표시
        CheckAnswer();        // 정답 여부 확인
    }

    void Update()
    {
        // 왼쪽 방향키 입력
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLever--;

            // 첫 번째 레버보다 왼쪽으로는 이동하지 못함
            if (currentLever < 0)
                currentLever = 0;

            UpdateSelection();   // 선택 표시 갱신
            CheckAnswer();       // 정답 확인
        }

        // 오른쪽 방향키 입력
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLever++;

            // 마지막 레버보다 오른쪽으로는 이동하지 못함
            if (currentLever > 7)
                currentLever = 7;

            UpdateSelection();
        }

        // 스페이스바를 누르면 현재 레버 상태 변경
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 레버 상태 반전
            leverState[currentLever] = !leverState[currentLever];

            // 상태에 맞는 이미지 표시
            levers[currentLever].down.SetActive(!leverState[currentLever]);
            levers[currentLever].up.SetActive(leverState[currentLever]);

            UpdateSelection();
            CheckAnswer();
        }
    }

    void UpdateSelection()
    {
        // 모든 레버의 색상을 갱신
        for (int i = 0; i < levers.Length; i++)
        {
            Image downImage = levers[i].down.GetComponent<Image>();
            Image upImage = levers[i].up.GetComponent<Image>();

            // 현재 선택된 레버는 흰색, 나머지는 회색으로 표시
            Color color = (i == currentLever)
                ? Color.white
                : new Color(0.5f, 0.5f, 0.5f, 1f);

            downImage.color = color;
            upImage.color = color;
        }
    }
    void CheckAnswer()
    {
        // 퍼즐 정답
        bool[] answer =
        {
            true,
            false,
            false,
            true,
            false,
            false,
            false,
            true
        };

        // 하나라도 다르면 아직 정답이 아니므로 함수 종료
        for (int i = 0; i < 8; i++)
        {
            if (leverState[i] != answer[i])
                return;
        }

        // 모든 레버가 정답과 일치하면 퍼즐 완료
        questManager.blackoutStage = 2;
        problemManager.isProblem = false;
        lastProblemUI.SetActive(false);
        blackPanel.SetActive(false);
    }
}