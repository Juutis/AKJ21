using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICost : MonoBehaviour
{
    [SerializeField]
    private Image imgIcon;
    [SerializeField]
    private TextMeshProUGUI txtAmount;

    public void Initialize(ShopCostResource cost)
    {
        imgIcon.sprite = cost.Icon;
        txtAmount.text = cost.Amount.ToString();
    }
}
