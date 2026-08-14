using UnityEngine;

public class Sungshin_ItemTip : MonoBehaviour
{
    [SerializeField]
    private Sungshin_TooltipUI tooltip;


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