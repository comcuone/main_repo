using UnityEngine;
using TMPro;

public class nan_GrillManager : MonoBehaviour
{
    public static nan_GrillManager Instance;

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text messageText;

    private float timer = 3f;

    private bool isCooking = false;
    private bool canFlip = false;
    private bool firstSide = true;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isCooking)
            return;

        if (!canFlip)
        {
            timer -= Time.deltaTime;

            timerText.text = Mathf.Ceil(timer).ToString();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                messageText.text = "아직 안 익었습니다!";
            }

            if (timer <= 0)
            {
                timerText.text = "";
                messageText.text = "SPACE를 눌러 뒤집으세요!";

                canFlip = true;
            }
        }

        if (canFlip && Input.GetKeyDown(KeyCode.Space))
        {
            canFlip = false;

            if (firstSide)
            {
                firstSide = false;

                FindFirstObjectByType<nan_SkewerMover>().Flip();

                FindFirstObjectByType<IngredientSelector>().ChangeToHalfCook();

                StartCooking();
            }
            else
            {
                // ⭐ 완전히 익은 이미지
                FindFirstObjectByType<IngredientSelector>().ChangeToDoneCook();

                // ⭐ 소스 위치로 이동
                FindFirstObjectByType<nan_SkewerMover>().MoveToSauce();

                // ⭐ 굽기 종료
                isCooking = false;
                timerText.text = "";
                messageText.text = "";
            }
        }
    }

    public void StartCooking()
    {
        timer = 3f;

        isCooking = true;
        canFlip = false;

        timerText.text = "3";
        messageText.text = "";
    }
}