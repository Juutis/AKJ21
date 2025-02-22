using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory: MonoBehaviour {

    private List<InventoryResource> resources = new();
    private List<ShipUpgradeConfigScriptableObject> upgrades = new();

    private int defaultStorage = 20;
    public bool IsShipInventory = false;
    public int CurrentWeight { get { return resources.Sum(r => r.Weight); } }

    public int GetMaxStorage() {
        if (!IsShipInventory){
            return int.MaxValue;
        }
        var storageUpgrade = upgrades.FirstOrDefault(u => u.UpgradeType == ShipUpgradeType.Storage);
        if (storageUpgrade != null) {
            return defaultStorage + storageUpgrade.IntValue;
        }
        return defaultStorage;
    }

    public int GetAmount(ResourceType resourceType) {
        var existingResource = resources.FirstOrDefault(r => r.ResourceType == resourceType);
        return existingResource != null ? existingResource.Amount : 0;
    }

    public bool AddResource(Resource resource, int amount) {
        UIManager.main.ShowMessage($"Adding resource: {resource.Name} amount: {amount}");
        if (IsShipInventory) {
            return AddResourceToShipStorage(resource, amount);
        } else {
            var newResource = AddResourceToStorage(resource, amount);
            Debug.Log("NewResource should not be null... " + newResource.Name);
            UIManager.main.AddResourceToBaseInventory(newResource);
            return true;
        }
    }

    private bool AddResourceToShipStorage(Resource resource, int amount) {
        if (CurrentWeight + resource.Weight * amount > GetMaxStorage()) {
            UIManager.main.ShowMessage("Not enough space in storage");
            return false;
        }

        var newResource = AddResourceToStorage(resource, amount);
        UIManager.main.AddResourceToShipInventory(newResource);
        return true;
    }

    private InventoryResource AddResourceToStorage(Resource resource, int amount) {
        var existingResource = resources.FirstOrDefault(r => r.ResourceType == resource.ResourceType);
        if (existingResource != null) {
            existingResource.Add(amount);
            return existingResource;
        } else {
            var newResource = new InventoryResource(resource, amount);
            resources.Add(newResource);
            return newResource;
        }
    }

    public bool Consume(ResourceType resourceType, int amount)
    {
        var resource = resources.FirstOrDefault(r => r.ResourceType == resourceType);
        if (resource == null) {
            return false;
        }
        bool consumeSuccesful = resource.Consume(amount);
        if (consumeSuccesful) {
            if (IsShipInventory) {
                UIManager.main.UpdateInventoryResourceView(resource);
            } else {
                UIManager.main.UpdateBaseResourceView(resource);
            }
        }
        return consumeSuccesful;
    }

    public List<InventoryResource> GetAll() {
        return resources;
    }

    // for testing
    public void AddHydrogen() {
        AddResource(ResourceManager.main.GetResource(ResourceType.Hydrogen), 10);
    }
    // for testing

    public void AddTitanium() {
        AddResource(ResourceManager.main.GetResource(ResourceType.Titanium), 1);
    }

    public void AddCrystal()
    {
        AddResource(ResourceManager.main.GetResource(ResourceType.Crystal), 1);
    }
    // for testing
}


public class InventoryResource
{
    public Resource Resource { get; private set; }
    public string Name { get { return Resource.Name; } }
    public string Description { get { return Resource.Description; } }
    public int Amount { get; private set; }
    public int Weight { get { return Resource.Weight * Amount; } }
    public Sprite Icon { get { return Resource.Icon; } }
    public ResourceType ResourceType { get { return Resource.ResourceType; } }
    public InventoryResource(Resource resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }
    public void Add(int amount)
    {
        Amount += amount;
    }

    public bool CanAfford(int amount)
    {
        return Amount >= amount;
    }

    public bool Consume(int amount) {
        if (CanAfford(amount)) {
            Amount -= amount;
            return true;
        }
        return false;
    }

    public void Clear()
    {
        Amount = 0;
    }
}