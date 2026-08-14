using UnityEngine;

public class nan_SkewerMover : MonoBehaviour
{
    public RectTransform grillPoint;
    public RectTransform saucePoint;

    private RectTransform target;
    public float moveSpeed = 600f;

    private RectTransform rect;
    private bool isMoving = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!isMoving)
            return;

        rect.anchoredPosition = Vector2.MoveTowards(
            rect.anchoredPosition,
            target.anchoredPosition,
            moveSpeed * Time.deltaTime);

        if (Vector2.Distance(rect.anchoredPosition, target.anchoredPosition) < 1f)
        {
            rect.anchoredPosition = target.anchoredPosition;
            isMoving = false;

            if (target == grillPoint)
            {
                Debug.Log("그릴 도착!");
                nan_GrillManager.Instance.StartCooking();
            }
            else if (target == saucePoint)
            {
                Debug.Log("소스 위치 도착!");

                nan_SauceSelector selector = FindFirstObjectByType<nan_SauceSelector>();

                selector.gameObject.SetActive(true);
                selector.EnableSelection();
            }
        }
    }

    public void MoveToGrill()
    {
        target = grillPoint;
        isMoving = true;
    }

    public void MoveToSauce()
    {
        target = saucePoint;
        isMoving = true;
    }

    public void Flip()
    {
        RectTransform rect = GetComponent<RectTransform>();

        Vector3 scale = rect.localScale;
        scale.x *= -1;
        rect.localScale = scale;
    }
}