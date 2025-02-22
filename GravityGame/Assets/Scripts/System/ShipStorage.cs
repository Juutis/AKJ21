using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipStorage: MonoBehaviour
{
    private List<InventoryResource> resources = new List<InventoryResource>();

    public int MaxWeight { get; private set; }
    public int CurrentWeight { get { return resources.Sum(r => r.Weight); } }

    public void AddResource(Resource resource, int amount)
    {
        if (CurrentWeight + resource.Weight * amount > MaxWeight)
        {
            UIManager.main.ShowMessage("Not enough space in storage");
        }

        var existingResource = resources.FirstOrDefault(r => r.Resource == resource);
        if (existingResource != null)
        {
            existingResource.Add(amount);
        }
        else
        {
            resources.Add(new InventoryResource ( resource, amount ));
        }
    }
}
