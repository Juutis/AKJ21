using System.Collections.Generic;
using UnityEngine;

public class Shop: MonoBehaviour {
    public bool Buy(Resource resource) {

        return false;
    }
}


[System.Serializable]
public class ShopCost {
    [SerializeField]
    private List<ShopCostResource> costs;

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
    public ResourceType ResourceType {get {return resourceType;}}
    [SerializeField]
    private int amount;
    public int Amount { get; private set; }
    public ShopCostResource (ResourceType type, int newAmount) {
        resourceType = type;
        amount = newAmount;
    }
}

[System.Serializable]
public class ShopUnlockRequirements
{

}