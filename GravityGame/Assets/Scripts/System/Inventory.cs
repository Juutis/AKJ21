using System.Collections.Generic;
using UnityEngine;

public class Inventory: MonoBehaviour {

    private List<InventoryResource> resources = new List<InventoryResource>();
    private List<ShipUpgradeConfigScriptableObject> upgrades = new List<ShipUpgradeConfigScriptableObject>();

    public int GetAmount(ResourceType resourceType) {
        var existingResource = resources.Find(r => r.ResourceType == resourceType);
        return existingResource != null ? existingResource.Amount : 0;
    }
}


public class InventoryResource
{
    public Resource Resource { get; private set; }
    public int Amount { get; private set; }
    public int Weight { get { return Resource.Weight * Amount; } }
    public ResourceType ResourceType { get { return Resource.Type; } }
    public InventoryResource(Resource resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }
    public void Add(int amount)
    {
        Amount += amount;
    }
    public void Clear()
    {
        Amount = 0;
    }
}