using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Image")]
    public Sprite interactSprite;

    [Header("Second State - 필요한 NPC만 설정")]
    public bool hasSecondState = false;
    public Sprite secondNormalSprite;
    public Sprite secondInteractSprite;

    [Header("Dialogue")]
    public int[] eventList;

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    private bool playerInRange = false;
    private bool isSecondState = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 게임 시작 시 이미지를 기본 이미지로 저장
        originalSprite = spriteRenderer.sprite;
    }

    void Update()
    {
        if (!playerInRange)
            return;

        if (DialogueManager.Instance == null)
        {
            Debug.LogError(gameObject.name + ": DialogueManager.Instance가 null 입니다.");
        }
        if (!DialogueManager.Instance.IsDialoguePlaying &&
            Input.GetKeyDown(KeyCode.E))
        {
            int step = GameManager.Instance.storyStep;

            if (step < eventList.Length)
            {
                DialogueManager.Instance.StartEvent(eventList[step]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // 두 번째 상태라면 두 번째 접근 이미지
            if (hasSecondState && isSecondState)
            {
                if (secondInteractSprite != null)
                    spriteRenderer.sprite = secondInteractSprite;
            }
            // 기존 상태라면 기존 접근 이미지
            else
            {
                if (interactSprite != null)
                    spriteRenderer.sprite = interactSprite;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // 두 번째 상태라면 두 번째 평소 이미지
            if (hasSecondState && isSecondState)
            {
                if (secondNormalSprite != null)
                    spriteRenderer.sprite = secondNormalSprite;
            }
            // 기존 상태라면 처음 이미지
            else
            {
                spriteRenderer.sprite = originalSprite;
            }
        }
    }

    // Event 16 종료 후 호출
    public void ChangeToSecondState()
    {
        if (!hasSecondState)
            return;

        isSecondState = true;

        // 현재 플레이어가 NPC 근처에 있다면
        if (playerInRange)
        {
            if (secondInteractSprite != null)
                spriteRenderer.sprite = secondInteractSprite;
        }
        else
        {
            if (secondNormalSprite != null)
                spriteRenderer.sprite = secondNormalSprite;
        }
    }
}