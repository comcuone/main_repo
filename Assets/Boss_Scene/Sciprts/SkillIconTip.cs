using UnityEngine;
using UnityEngine.EventSystems;

//마우스를 올렸을 때 설명창을 띄우는 스크립트입니다.
public class SkillIconTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private BossTooltipUI tooltip;

    [SerializeField]
    private string itemDescription;

    //마우스를 올리면 설명창을 켭니다.
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Show(itemDescription);
    }

    //마우스를 떼면 설명창을 끕니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}