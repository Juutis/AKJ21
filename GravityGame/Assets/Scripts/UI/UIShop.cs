using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIShop : MonoBehaviour
{
    [SerializeField]
    private UIShopItem uiShopItemPrefab;

    [SerializeField]
    private Transform elementContainer;

    private List<UIShopItem> shopItems = new();

    public void Initialize(List<ShopItem> items)
    {
        foreach (var item in items)
        {
            var shopItem = Instantiate(uiShopItemPrefab, elementContainer);
            shopItem.Initialize(item);
            shopItems.Add(shopItem);
        }
    }
    public void MarkShopItemAsBought(ShopItem shopItem) {
        var item = shopItems.FirstOrDefault(i => i.ShopItem == shopItem);
        if (item != null) {
            item.MarkAsBought();
        }
    }

    public void ShowItem(ShopItem shopItem) {
        
    }
}
