using UnityEngine;

// 플레이어와의 거리를 확인하여 이미지 변경 및 상호작용 여부를 관리하는 스크립트
public class Objects : MonoBehaviour
{
    public Sungshin_Player Player;
    // 플레이어 스크립트

    [SerializeField]
    private Transform player;
    // 플레이어 위치

    private SpriteRenderer sr;
    // 현재 오브젝트의 SpriteRenderer

    [SerializeField]
    private Sprite image1;
    // 기본 이미지

    [SerializeField]
    private Sprite image2;
    // 플레이어가 가까이 왔을 때 이미지

    [SerializeField]
    private Sprite image3;
    // 이벤트 후 기본 이미지

    [SerializeField]
    private Sprite image4;
    // 이벤트 후 플레이어가 가까이 왔을 때 이미지

    [SerializeField]
    private float distance;
    // 상호작용이 가능한 거리

    public bool canInteract;
    // 현재 플레이어와 상호작용 가능한지 여부

    private bool changed = false;
    // 이벤트로 이미지가 변경되었는지 여부

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 플레이어가 상호작용 거리 안에 있는 경우
        if (Vector2.Distance(transform.position, player.position) <= distance)
        {
            // 현재 상태에 맞는 이미지로 변경
            if (sr.sprite != (changed ? image4 : image2))
            {
                sr.sprite = changed ? image4 : image2; //changed == true라면 image4를, 아니라면 image2 사용
            }

            canInteract = true;            // 상호작용 가능
            Player.pObject = gameObject;   // 현재 상호작용 가능한 오브젝트 저장
        }
        else
        {
            // 플레이어가 멀어졌다면 기본 이미지로 변경
            if (sr.sprite != (changed ? image3 : image1))
            {
                sr.sprite = changed ? image3 : image1; //changed == true라면 image3을, 아니라면 image1 사용
            }

            canInteract = false;   // 상호작용 불가

            // 현재 선택된 오브젝트라면 해제
            if (Player.pObject == gameObject)
            {
                Player.pObject = null;
            }
        }
    }

    public void ChangeClothes()
    {
        changed = true;   // 이벤트 이후 이미지 변경 상태로 설정

        // 현재 플레이어와의 거리에 맞는 변경된 이미지 적용
        if (Vector2.Distance(transform.position, player.position) <= distance)
            sr.sprite = image4;
        else
            sr.sprite = image3;
    }
}