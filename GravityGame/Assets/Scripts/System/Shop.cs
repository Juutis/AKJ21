using System.Collections.Generic;
using UnityEngine;

public class Shop: MonoBehaviour {
    [SerializeField]
    private Inventory baseInventory;
    [SerializeField]
    private Inventory shipInventory;
    [SerializeField]
    private ShipUpgradeManager shipUpgradeManager;

    [SerializeField]
    private ShopItem shopItemPrefab;

    [SerializeField]
    private Transform elementContainer;
    [SerializeField]
    private List<ShipUpgradeConfigScriptableObject> upgrades = new();
    private List<ShopItem> shopItems = new();

    private void Start() {
        foreach (var upgrade in upgrades) {
            ShopItem shopItem = Instantiate(shopItemPrefab, elementContainer);
            shopItem.Initialize(upgrade);
            shopItems.Add(shopItem);
        }
        UIManager.main.InitializeShop(shopItems);
        UIManager.main.InitializeBaseInventory(baseInventory);
        UIManager.main.InitializeShipInventory(shipInventory);
    }

    public bool Buy(ShopItem shopItem) {
        Debug.Log($"Buying {shopItem.Name}");
        if (shopItem.IsBought) {
            Debug.Log($"Already bought!");
            return false;
        }
        if (shopItem.Cost.CanBuy(baseInventory)) {
            foreach (var cost in shopItem.Cost.Costs) {
                baseInventory.Consume(cost.ResourceType, cost.Amount);
            }
            shopItem.Buy();
            shipUpgradeManager.ApplyUpgrade(shopItem);
            UIManager.main.MarkShopItemAsBought(shopItem);
            Debug.Log($"Purchase succesful!");
            return true;
        }
        Debug.Log($"Couldn't afford it!");
        return false;
    }

    public void TransferShipResourcesToBase() {
        foreach (var resource in shipInventory.GetAll()) {
            baseInventory.AddResource(resource.Resource, resource.Amount);
            shipInventory.Consume(resource.Resource.ResourceType, resource.Amount);
        }
    }

    public void BuyTest() {
        var shopItem = shopItems[0];
        if (Buy(shopItem)) {
            UIManager.main.ShowMessage("Bought " + shopItem.Name);
        } else {
            UIManager.main.ShowMessage("Can't afford " + shopItem.Name);
        }
    }
}


[System.Serializable]
public class ShopCost {
    [SerializeField]
    private List<ShopCostResource> costs;

    public List<ShopCostResource> Costs { get { return costs; } }

    public bool CanBuy(Inventory inventory) {
        foreach (var cost in costs) {
            if (inventory.GetAmount(cost.ResourceType) < cost.Amount) {
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public class ShopCostResource {
    [SerializeField]
    private ResourceType resourceType;
    public ResourceType ResourceType { get { return resourceType; } }
    [SerializeField]
    private int amount;
    public int Amount { get { return amount; } }
    private Resource resource { get { return ResourceManager.main.GetResource(resourceType); } }
    public Sprite Icon  { get { return resource.Icon; } }
}

[System.Serializable]
public class ShopUnlockRequirements
{

}