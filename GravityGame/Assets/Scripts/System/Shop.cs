using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class Shop: MonoBehaviour {
    public static Shop main;

    void Awake()
    {
        if (main == null)
        {
            main = this;
            foreach (var upgrade in upgrades)
            {
                ShopItem shopItem = Instantiate(shopItemPrefab, elementContainer);
                shopItem.Initialize(upgrade);
                shopItems.Add(shopItem);
            }
        }
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
    private float distanceFromShopToEnter = 20f;


    [SerializeField]
    private Transform elementContainer;
    [SerializeField]
    private List<ShipUpgradeConfigScriptableObject> upgrades = new();
    private List<ShopItem> shopItems = new();

    private void Start() {
        if (main != this) {
            return;
        }
        Debug.Log("i am this");

#if UNITY_EDITOR
        //AddResourceToShip(ResourceType.Crystal, 2);
        baseInventory.AddResource(ResourceManager.main.GetResource(ResourceType.Crystal), 24);
        baseInventory.AddResource(ResourceManager.main.GetResource(ResourceType.Titanium), 24);
        baseInventory.AddResource(ResourceManager.main.GetResource(ResourceType.Hydrogen), 20);
        //AddResourceToShip(ResourceType.Hydrogen, 10);
#endif
    }

    void OnEnable()
    {
        if (main != this)
        {
            return;
        }
        Debug.Log("OnEnable called from shop");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (main != this)
        {
            return;
        }
        UIManager.main.InitializeShop(shopItems);
        UIManager.main.InitializeBaseInventory(baseInventory);
        UIManager.main.InitializeShipInventory(shipInventory);
        Debug.Log("UI initialized from shop");
    }

    public bool CanBuy(ShopItem shopItem) {
        return shopItem.Cost.CanBuy(baseInventory);
    }

    private Transform GetBase() {
        var homeBase = GameObject.FindGameObjectWithTag("base");
        return homeBase.transform;
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

    public string TransferShipResourcesToBase() {
        List<string> addedResources = new();
        foreach (var resource in shipInventory.GetAll()) {
            baseInventory.AddResource(resource.Resource, resource.Amount);
            if (resource.Amount > 0) {
                addedResources.Add($"{resource.Amount} {resource.Name}");
            }
            shipInventory.Consume(resource.Resource.ResourceType, resource.Amount);
        }
        UIManager.main.HideStorageIsFull();
        if (addedResources.Count > 0) {
            return $"Ship to Base: {string.Join(" | ", addedResources)}";
        } else {
            return "Ship had no resources to transfer to base";
        }
    }


    public bool IsInRangeOfShop() {
        if  (GetBase() == null) {
            return false;
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            return false;
        }
        if (player.GetComponent<ShipControls>().isDead) {
            return false;
        }
        return Vector3.Distance(GetBase().position, player.transform.position) <= distanceFromShopToEnter;
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