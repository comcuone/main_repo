using UnityEngine;
using TMPro;

// 아이템 설명을 입력받아 설명창을 켜고 끕니다.

public class BossTooltipUI : MonoBehaviour
{

    [SerializeField]
    private TMP_Text itemDescription;

    //시작할 때는 숨겨 둡니다.
    void Start()
    {
        gameObject.SetActive(false);
    }

    //설명창을 보여줍니다.
    public void Show(string description)
    {
        gameObject.SetActive(true);
        itemDescription.text = description;
    }

    //설명창을 숨깁니다.
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
