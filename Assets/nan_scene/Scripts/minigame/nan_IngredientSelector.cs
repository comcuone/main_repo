using UnityEngine;
using UnityEngine.UI;


public class IngredientSelector : MonoBehaviour
{
    [Header("선택 테두리")]
    public GameObject[] outlines;

    private int currentIndex = 0;
    // 플레이어가 선택한 재료 저장

    [Header("슬롯")]
    public Image[] slots;

    [Header("재료 스프라이트")]
    public Sprite[] ingredientSprites;
    [Header("반쯤 익음")]
    public Sprite[] halfCookSprites;

    [Header("완전히 익음")]
    public Sprite[] doneCookSprites;

    [Header("빈 슬롯 이미지")]
    public Sprite emptySprite;
    private int currentSlot = 0;

    // 플레이어가 선택한 재료
    private nan_IngredientType[] playerOrder = new nan_IngredientType[3];

    public nan_OrderManager nan_OrderManager;

    

    void Start()
    {
        UpdateOutline();
        for (int i = 0; i < playerOrder.Length; i++)
        {
            playerOrder[i] = (nan_IngredientType)(-1);
        }
    }

    void Update()
    {
        // 위 방향
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;

            if (currentIndex < 0)
                currentIndex = outlines.Length - 1;

            UpdateOutline();
        }

        // 아래 방향
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;

            if (currentIndex >= outlines.Length)
                currentIndex = 0;

            UpdateOutline();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectIngredient();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveIngredient();
        }
    }

    void UpdateOutline()
    {
        for (int i = 0; i < outlines.Length; i++)
        {
            outlines[i].SetActive(i == currentIndex);
        }
    }

    public int GetSelectedIndex()
    {
        return currentIndex;
    }

    void SelectIngredient()
    {
        if (currentSlot >= slots.Length)
            return;

        slots[currentSlot].sprite = ingredientSprites[currentIndex];

        playerOrder[currentSlot] = (nan_IngredientType)currentIndex;

        currentSlot++;

        if (currentSlot >= 3)
        {
            CheckOrder();
        }
    }

    void RemoveIngredient()
    {
        if (currentSlot <= 0)
            return;

        currentSlot--;

        slots[currentSlot].sprite = emptySprite;

        playerOrder[currentSlot] = (nan_IngredientType)(-1);
    }

    void CheckOrder()
    {
        nan_IngredientType[] order = nan_OrderManager.GetCurrentOrder();

        bool success = true;

        for(int i = 0; i < 3; i++)
        {
            if(playerOrder[i] != order[i])
            {
                success = false;
                break;
            }
        }

        if(success)
        {
            Debug.Log("성공!");

            enabled = false;
            
            FindFirstObjectByType<nan_SkewerMover>().MoveToGrill();
        }
        else
        {
            Debug.Log("틀렸습니다.");
        }
    }

    public void ChangeToHalfCook()
    {
        for(int i=0;i<3;i++)
        {
            slots[i].sprite = halfCookSprites[(int)playerOrder[i]];
        }
    }

    public void ChangeToDoneCook()
    {
        for(int i=0;i<3;i++)
        {
            slots[i].sprite = doneCookSprites[(int)playerOrder[i]];
        }
    }
}