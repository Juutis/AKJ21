using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItemDetails : MonoBehaviour
{
    [SerializeField]
    private Image imgIcon;
    [SerializeField]
    private TextMeshProUGUI txtName;
    [SerializeField]
    private TextMeshProUGUI txtDescription;

    [SerializeField]
    private UICost uiCostPrefab;

    [SerializeField]
    private Transform elementContainer;

    private List<UICost> costs = new(); 

    public void Initialize(ShopItem shopItem)
    {
        Debug.Log($"Initializing {shopItem.Name}");
        imgIcon.sprite = shopItem.Icon;
        txtName.text = shopItem.Name;
        txtDescription.text = shopItem.Description;

        foreach (var cost in shopItem.Cost.Costs)
        {
            var uiCost = Instantiate(uiCostPrefab, elementContainer);
            uiCost.Initialize(cost);
            costs.Add(uiCost);
        }
    }
}
