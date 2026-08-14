using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    // Update is called once per frame
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
