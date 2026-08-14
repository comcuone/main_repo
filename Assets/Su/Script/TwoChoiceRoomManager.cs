using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TwoChoiceRoomManager : MonoBehaviour
{
    public static TwoChoiceRoomManager Instance;

    [Header("Player")]
    public Transform player;
    public Transform quizSpawnPoint;
    private Vector3 SpawnPoint;
    private Vector3 originalScale;

    [Header("Camera")]
    public Camera mainCamera;
    public CameraMove cameraMove;
    public Transform quizCameraPoint;

    [Header("Quiz")]
    public TMP_Text quizText;


    public TMP_Text leftDoorText;
    public TMP_Text rightDoorText;

    public QuizData[] quizzes;

    private int currentQuiz = 0;
    private Vector3 originalCameraPosition;

    

    private void Awake()
    {
        Instance = this;
    }

    public void EnterQuizRoom()
    {
        SpawnPoint = player.position;
        originalScale = player.localScale;
        // 플레이어 이동
        player.position = quizSpawnPoint.position;
        player.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        originalCameraPosition = mainCamera.transform.position;
        // 카메라 추적 중지
        cameraMove.followPlayer = false;

        // 카메라를 퀴즈방 위치로 이동
        mainCamera.transform.position = new Vector3(
            quizCameraPoint.position.x,
            quizCameraPoint.position.y,
            mainCamera.transform.position.z
        );

        ShowQuiz(0);
    }

    public void ShowQuiz(int index)
    {
        if (index < 0 || index >= quizzes.Length)
            return;
            
        currentQuiz = index;
        QuizData quiz = quizzes[index];
        quizText.text = quiz.quizText;

        for (int i = 0; i<quizzes.Length; i++)
        {
            if (quizzes[i].questionImage != null)
            {
                quizzes[i].questionImage.SetActive(false);
            }
        }

        if (quiz.questionImage != null)
            quiz.questionImage.SetActive(true);


        leftDoorText.text = quiz.leftChoice;
        rightDoorText.text = quiz.rightChoice;
    }

    public bool CheckAnswer(bool chooseLeft)
    {
        return quizzes[currentQuiz].leftIsCorrect == chooseLeft;
    }

    public void NextQuiz()
    {
        currentQuiz++;

        if (currentQuiz >= quizzes.Length)
        {
            DialogueManager.Instance.StartEvent(15);
            return;
        }

        player.position = quizSpawnPoint.position;
        ShowQuiz(currentQuiz);
    }

    public void SelectDoor(bool chooseLeft)
    {
        bool isCorrect = CheckAnswer(chooseLeft);
        if (isCorrect)
        {
            NextQuiz();
        }
        else
        {
            DialogueManager.Instance.StartEvent(18);
        }
    }

    public void ExitQuizRoom()
    {
        currentQuiz = 0;
        player.localScale = originalScale;
        // 플레이어를 원래 위치로 이동
        player.position = SpawnPoint;
        mainCamera.transform.position = originalCameraPosition;

        // 다시 플레이어 추적
        cameraMove.followPlayer = true;
    }
}