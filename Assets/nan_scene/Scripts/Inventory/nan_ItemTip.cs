using UnityEngine;

public class nan_ItemTip : MonoBehaviour
{
    [SerializeField]
    private nan_TooltipUI tooltip;


    [SerializeField]
    private string itemDescription;

    void OnMouseEnter()
    {
        tooltip.Show(itemDescription);
    }

    void OnMouseExit()
    {
        tooltip.Hide();
    }
}