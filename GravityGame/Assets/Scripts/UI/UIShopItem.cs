using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class UIShopItem : MonoBehaviour
{
    private ShopItem shopItem;
    public ShopItem ShopItem { get { return shopItem; } }

    [SerializeField]
    private Image imgIcon;
    [SerializeField]
    private TextMeshProUGUI txtName;
    [SerializeField]
    private TextMeshProUGUI txtDescription;

    [SerializeField]
    private Transform elementContainer;

    [SerializeField]
    private UICost uiCostPrefab;
    private List<UICost> costs = new();

    private bool isBought = false;
    public void Initialize(ShopItem newShopItem)
    {
        shopItem = newShopItem;
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

    public void MarkAsBought() {
        imgIcon.color = Color.red;
        isBought = true;
    }


}
