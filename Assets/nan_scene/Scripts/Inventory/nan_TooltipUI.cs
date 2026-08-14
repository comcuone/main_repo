using UnityEngine;
using TMPro;

public class nan_TooltipUI : MonoBehaviour
{

    [SerializeField]
    private TMP_Text itemDescription;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(string description)
    {
        gameObject.SetActive(true);
        itemDescription.text = description;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}