using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;

    public float minX;
    public float maxX;

    public bool followPlayer = true;   // 추가

    void LateUpdate()
    {
        if (!followPlayer) return;

        if (target == null) return;

        float x = Mathf.Clamp(target.position.x, minX, maxX);

        transform.position = new Vector3(
            x,
            transform.position.y,
            transform.position.z
        );
    }
}