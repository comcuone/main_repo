using UnityEngine;

public class nan_PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 대화 중이면 움직이지 못하게 하기
        if (nan_DialogueManager.Instance != null && nan_DialogueManager.Instance.IsOpen)
        {
            moveX = 0f;
            animator.SetFloat("Speed", 0f);
            return;
        }

        // A, D 입력만 받기
        moveX = Input.GetAxisRaw("Horizontal");

        // 이동 애니메이션
        animator.SetFloat("Speed", Mathf.Abs(moveX));

        // 바라보는 방향 변경
        if (moveX > 0)
        {
            spriteRenderer.flipX = true;   // 오른쪽
        }
        else if (moveX < 0)
        {
            spriteRenderer.flipX = false;  // 왼쪽
        }
    }

    void FixedUpdate()
    {
        // 대화 중이면 완전히 정지
        if (nan_DialogueManager.Instance != null && nan_DialogueManager.Instance.IsOpen)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }
}