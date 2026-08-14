using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private float move;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialoguePlaying)
        {
            move = 0;
            anim.SetFloat("Speed", 0);
            return;
        }

        move = Input.GetAxisRaw("Horizontal");

        // 걷기 애니메이션
        anim.SetFloat("Speed", Mathf.Abs(move));

        // 좌우 방향 뒤집기
        if (move < 0)
            sr.flipX = false;
        else if (move > 0)
            sr.flipX = true;
    }

    void FixedUpdate()
    {
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialoguePlaying)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(
            move * moveSpeed,
            rb.linearVelocity.y
        );
    }
}

