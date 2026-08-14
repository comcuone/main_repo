using UnityEngine;

//플레이어가 위아래로 움직이게 하는 스크립트입니다.
public class BossPlayer : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    void Update()
    {
        if (BossGameManager.Instance.IsDialogPlaying)
            return;
        //float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 moveTo = new Vector3(0f, verticalInput, 0f);
        transform.position += moveTo * moveSpeed * Time.deltaTime;
    }
}