using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Shop: MonoBehaviour {
    public static Shop main;

    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private Inventory baseInventory;
    [SerializeField]
    private Inventory shipInventory;
    [SerializeField]
    private ShipUpgradeManager shipUpgradeManager;

    [SerializeField]
    private ShopItem shopItemPrefab;


    [SerializeField]
    private Transform homeBase;

    [SerializeField]
    private float distanceFromShopToEnter = 20f;


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

    public bool CanBuy(ShopItem shopItem) {
        return shopItem.Cost.CanBuy(baseInventory);
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

    public bool AddResourceToShip(ResourceType resourceType, int amount) {
        return shipInventory.AddResource(ResourceManager.main.GetResource(resourceType), amount);
    }


    public void TransferShipResourcesToBase() {
        List<string> addedResources = new();
        foreach (var resource in shipInventory.GetAll()) {
            baseInventory.AddResource(resource.Resource, resource.Amount);
            if (resource.Amount > 0) {
                addedResources.Add($"{resource.Amount} {resource.Name}");
            }
            shipInventory.Consume(resource.Resource.ResourceType, resource.Amount);
        }
        if (addedResources.Count > 0) {

            UIManager.main.ShowMessage($"Ship to Base: {string.Join(" | ", addedResources)}");
        } else {
            UIManager.main.ShowMessage("Ship had no resources to transfer to base");
        }
        UIManager.main.HideStorageIsFull();
    }


    public bool IsInRangeOfShop() {
        if  (homeBase == null) {
            return false;
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            return false;
        }
        return Vector3.Distance(homeBase.position, player.transform.position) <= distanceFromShopToEnter;
    }

    void Update()
    {
        if (!IsInRangeOfShop()) {
            return;
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