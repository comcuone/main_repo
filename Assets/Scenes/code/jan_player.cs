using UnityEngine;
using UnityEngine.InputSystem;

public class jan_player : MonoBehaviour
{
    [SerializeField]
    public float speed = 3f;
    public jan_GameManager manager;
    
    // 오타 수정: scanObjet -> scanObject
    GameObject scanObject; 
    Vector3 move;

    // 플레이어가 바라보는 방향 저장 (기본값: 오른쪽)
    Vector3 dirVec = Vector3.right; 

    void Start()
    {
        
    }

    void Update()
    {
        move = Vector3.zero;

        // 1. 이동 입력 처리 및 바라보는 방향(dirVec) 업데이트
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
        {
            move += manager.isAction ? Vector3.zero : new Vector3(-1, 0, 0);
            dirVec = Vector3.left; // 왼쪽 바라봄
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) 
        {
            move += manager.isAction ? Vector3.zero : new Vector3(1, 0, 0);
            dirVec = Vector3.right; // 오른쪽 바라봄
        }

        // 2. 캐릭터 이미지 반전 (Flip)
        if (move.x > 0) 
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (move.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        // 3. 애니메이션 처리
        if (move.magnitude > 0)
        {
            GetComponent<Animator>().SetTrigger("move");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("stop");
        }

        // 4. [추가] 레이어 3번(Layer 3) 오브젝트 감지 로직
        // Scene 창에서 빨간색 선으로 감지 거리를 확인할 수 있습니다.
        Debug.DrawRay(transform.position, dirVec * 2f, new Color(0,1,0));

        // 1 << 3 은 Layer index 3번을 의미합니다.
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 2f, LayerMask.GetMask("object"));

        // 정면에 3번 레이어 콜라이더가 잡히면 scanObject에 할당
        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }

        // 5. E키를 눌렀을 때 감지된 오브젝트가 존재하면 상호작용
        if (Input.GetKeyDown(KeyCode.E) && scanObject != null)
        {
            manager.Action(scanObject);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(move * speed * Time.fixedDeltaTime);
    }
}