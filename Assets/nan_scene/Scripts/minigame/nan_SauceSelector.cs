using UnityEngine;
using UnityEngine.UI;

public class nan_SauceSelector : MonoBehaviour
{
    [Header("선택 테두리")]
    public GameObject[] outlines;

    private int currentIndex = 0;

    [Header("소스 슬롯")]
    public Image sauceSlot;

    [Header("소스 스프라이트")]
    public Sprite[] sauceSprites;

    [Header("빈 슬롯")]
    public Sprite emptySprite;

    public nan_OrderManager nan_OrderManager;

    private nan_SauceType selectedSauce;

    private bool canSelect = false;

    void Start()
    {
        UpdateOutline();

        selectedSauce = (nan_SauceType)(-1);

        sauceSlot.sprite = emptySprite;
    }

    void Update()
    {
        if (!canSelect)
            return;
        // 위
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;

            if (currentIndex < 0)
                currentIndex = outlines.Length - 1;

            UpdateOutline();
        }

        // 아래
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;

            if (currentIndex >= outlines.Length)
                currentIndex = 0;

            UpdateOutline();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectSauce();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveSauce();
        }
    }

    void UpdateOutline()
    {
        for (int i = 0; i < outlines.Length; i++)
        {
            outlines[i].SetActive(i == currentIndex);
        }
    }

    void SelectSauce()
    {
        sauceSlot.sprite = sauceSprites[currentIndex];

        selectedSauce = (nan_SauceType)currentIndex;

        CheckSauce();
    }

    void RemoveSauce()
    {
        sauceSlot.sprite = emptySprite;

        selectedSauce = (nan_SauceType)(-1);
    }

    void CheckSauce()
    {
        if (selectedSauce == nan_OrderManager.GetCurrentSauce())
        {
            Debug.Log("소스 성공!");

            FinishMinigame();

        }
        else
        {
            Debug.Log("소스 실패!");
        }
    }

    void FinishMinigame()
    {
        // 소스 선택창 종료
        gameObject.SetActive(false);

        // 미니게임 배경 종료
        FindFirstObjectByType<nan_DialogueManager>().minigameBackground.SetActive(false);

        // 대화 이어가기
        nan_DialogueManager.Instance.ResumeDialogue();
    }

    public void EnableSelection()
    {
        canSelect = true;
    }
}