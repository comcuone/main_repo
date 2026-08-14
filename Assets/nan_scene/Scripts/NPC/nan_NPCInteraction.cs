using UnityEngine;

public class nan_NPCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject outline;
    [SerializeField] private int startDialogueID;
    [SerializeField] private int stage2DialogueID;

    [SerializeField] private GameObject notebook;
    [SerializeField] private bool giveNotebook = false;

    [SerializeField] private bool finishAfterLastDialogue = false;

    private int currentStage = 1;

    private bool isFinished = false;

    private bool canInteract = false;

    private void Start()
    {
        if (outline != null)
            outline.SetActive(false);
    }

    private void Update()
    {
        if (!canInteract)
            return;

        // 대화 중이면 E 입력 무시
        if (nan_DialogueManager.Instance != null &&
            nan_DialogueManager.Instance.IsOpen)
            return;

        // 퀴즈 중이면 E 입력 무시
        if (nan_QuizManager.Instance != null &&
            nan_QuizManager.Instance.IsQuizOpen)
            return;

        if (Input.GetKeyDown(KeyCode.E) && !isFinished)
        {
            Debug.Log($"E 눌림!");
            Debug.Log($"대화 시작 Stage : {currentStage}");

            if (nan_DialogueManager.Instance == null)
            {
                Debug.LogError("nan_DialogueManager.Instance가 없습니다!");
                return;
            }

            nan_DialogueManager.Instance.SetCurrentNPC(this);

            if (currentStage == 1)
            {
                nan_DialogueManager.Instance.StartDialogue(startDialogueID);
            }
            else if (currentStage == 2)
            {
                nan_DialogueManager.Instance.StartDialogue(stage2DialogueID);
            }
            else
            {
                Debug.Log("더 이상 대화가 없습니다.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        canInteract = true;

        if (outline != null)
            outline.SetActive(true);

        Debug.Log("NPC 범위 진입");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        canInteract = false;

        if (outline != null)
            outline.SetActive(false);

        if (nan_DialogueManager.Instance != null &&
            nan_DialogueManager.Instance.IsOpen)
        {
            nan_DialogueManager.Instance.CloseDialogue();
        }

        Debug.Log("NPC 범위 종료");
    }

    public void NextStage()
    {
        currentStage++;
        Debug.Log($"현재 Stage : {currentStage}");
    }

    public void FinishInteraction()
    {
        Debug.Log("FinishInteraction 호출");

        isFinished = true;
        canInteract = false;

        if (outline != null)
            outline.SetActive(false);

        if (notebook == null)
        {
            Debug.Log("Notebook이 연결 안 됨!");
            return;
        }

        Debug.Log("Notebook 활성화!");
        notebook.SetActive(true);

        Debug.Log("현재 활성화 여부 : " + notebook.activeSelf);
    }

    public bool ShouldFinish()
    {
        return finishAfterLastDialogue;
    }

}