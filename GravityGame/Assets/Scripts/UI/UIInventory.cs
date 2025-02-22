using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIInventory : MonoBehaviour
{
    [SerializeField]
    private UIInventoryItem uiInventoryItemPrefab;

    [SerializeField]
    private Transform elementContainer;
    private List<UIInventoryItem> items = new List<UIInventoryItem>();

    [SerializeField]
    private UIStorageIndicator storageIndicator;
    private bool isShipInventory;

    public void Initialize(Inventory inventory, bool IsShipInventory)
    {
        isShipInventory = IsShipInventory;
        items.Clear();
        if (IsShipInventory) {
            storageIndicator.Initialize(inventory);
        }
    }

    private UIInventoryItem GetItem(InventoryResource resource)
    {
        return items.FirstOrDefault(i => i.ResourceType == resource.ResourceType);
    }

    private UIInventoryItem GetOrCreateItem(InventoryResource resource)
    {
        var existingItem = GetItem(resource);
        if (existingItem != null)
        {
            return existingItem;
        }
        else
        {
            var newItem = Instantiate(uiInventoryItemPrefab, elementContainer);
            newItem.Initialize(resource);
            items.Add(newItem);
            return newItem;
        }
    }

    public void AddResource(InventoryResource resource)
    {
        var item = GetOrCreateItem(resource);
        item.UpdateAmount();
        if (isShipInventory) {
            storageIndicator.UpdateView();
        }
    }

    public void UpdateResourceView(InventoryResource resource) {
        var item = GetItem(resource);
        if (item == null) {
            return;
        }
        item.UpdateAmount();
        if (isShipInventory) {
            storageIndicator.UpdateView();
        }
    }
}