using UnityEngine;

public class Sungshin_Player : MonoBehaviour
{
    public Sungshin_DialogueManager dialogueManager;
    public Sungshin_ProblemManager problemManager;

    public GameObject pObject;

    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField]
    private float movespeed;

    private float direction;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 대화 또는 미니게임 중이면 이동 막기
        if (dialogueManager.isAction || problemManager.isProblem)
            direction = 0;
        else
            direction = Input.GetAxisRaw("Horizontal"); //좌우 방향키 입력을 받아 이동 방향을 결정

        anim.SetBool("isWalking", direction != 0); //방향키를 누르고 있을 때만 걷는 모션 실행

        if (direction > 0) //오른쪽으로 이동할 때 이미지를 좌우반전시켜 표시
            sr.flipX = true;
        else if (direction < 0)
            sr.flipX = false;

        // 미니게임 중에는 NPC와 대화 금지
        if (problemManager.isProblem)
            return;

        // 대화가 아닐 때는 E로 시작
        if (!dialogueManager.isAction &&
            Input.GetKeyDown(KeyCode.E) &&
            pObject != null &&
            !dialogueManager.isEvent)
        {
            dialogueManager.Action(pObject);
        }

        // 대화 중에는 Space로 진행
        if (dialogueManager.isAction &&
            Input.GetKeyDown(KeyCode.Space))
        {
            dialogueManager.Action(pObject);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * movespeed, 0f); //플레이어의 이동방향과 속도를 이용해 이동
    }
}