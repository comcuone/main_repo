using UnityEngine;

public class nan_OrderManager : MonoBehaviour
{
    [Header("주문서")]
    public GameObject[] orders;

    private int currentOrderIndex;
    private nan_SauceType currentSauce;

    private nan_IngredientType[][] orderData =
    {
        new nan_IngredientType[]
        {
            nan_IngredientType.RiceCake,
            nan_IngredientType.Onion,
            nan_IngredientType.Meat
        },

        new nan_IngredientType[]
        {
            nan_IngredientType.Meat,
            nan_IngredientType.RiceCake,
            nan_IngredientType.Onion
        },

        new nan_IngredientType[]
        {
            nan_IngredientType.Onion,
            nan_IngredientType.Meat,
            nan_IngredientType.RiceCake
        }
    };

    private nan_SauceType[] sauceData =
    {
        nan_SauceType.Seasoning,
        nan_SauceType.Mayo,
        nan_SauceType.Seasoning
    };
    void Start()
    {
        GenerateOrder();
    }

    public void GenerateOrder()
    {
        // 전부 끄기
        foreach (GameObject order in orders)
        {
            order.SetActive(false);
        }

        // 랜덤 선택
        currentOrderIndex = Random.Range(0, orders.Length);

        //현재 주문 소스 저장
        currentSauce = sauceData[currentOrderIndex];

        // 선택된 주문서만 켜기
        orders[currentOrderIndex].SetActive(true);
    }

    public int GetCurrentOrderIndex()
    {
        return currentOrderIndex;
    }

    public nan_IngredientType[] GetCurrentOrder()
    {
        return orderData[currentOrderIndex];
    }

    public nan_SauceType GetCurrentSauce()
    {
        return currentSauce;
    }
}