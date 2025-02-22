using System.Collections.Generic;
using UnityEngine;

public class UIManager: MonoBehaviour {
    public static UIManager main;

    [SerializeField]
    private UIInventory uiBaseInventory;
    [SerializeField]
    private UIInventory uiShipInventory;
    [SerializeField]
    private UIShop uiShop;

    void Awake()
    {
        main = this;
    } 

    void Start() {

    }

    public void InitializeBaseInventory(Inventory inventory) {
        uiBaseInventory.Initialize(inventory, false);
    }

    public void InitializeShipInventory(Inventory inventory) {
        uiShipInventory.Initialize(inventory, true);
    }

    public void InitializeShop(List<ShopItem> items) {
        uiShop.Initialize(items);
    }

    public void ShowMessage(string message)
    {
        Debug.Log(message);
    }

    public void AddResourceToBaseInventory(InventoryResource resource) {
        uiBaseInventory.AddResource(resource);
    }

    public void AddResourceToShipInventory(InventoryResource resource)
    {
        uiShipInventory.AddResource(resource);
    }

    public void UpdateBaseResourceView(InventoryResource resource) {
        uiBaseInventory.UpdateResourceView(resource);
    }
    public void UpdateInventoryResourceView(InventoryResource resource) {
        uiShipInventory.UpdateResourceView(resource);
    }

    public void ShowShopItem(ShopItem shopItem) {
        uiShop.ShowItem(shopItem);
    }

    public void MarkShopItemAsBought(ShopItem shopItem) {
        uiShop.MarkShopItemAsBought(shopItem);
    }

}