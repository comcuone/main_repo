using UnityEngine;

//마우스를 올렸을 때 설명창을 띄우는 스크립트입니다.
public class BossItemTip : MonoBehaviour
{
    [SerializeField]
    private BossTooltipUI tooltip;


    [SerializeField]
    private string itemDescription;

    //마우스가 닿으면 설명창을 띄웁니다.
    void OnMouseEnter()
    {
        //일시정지 중에는 뜨지 않게
        if (BossGameManager.Instance.IsPaused)
            return;
        tooltip.Show(itemDescription);
    }

    //마우스가 떨어지면 설명창을 숨깁니다.
    void OnMouseExit()
    {
        tooltip.Hide();
    }
}
