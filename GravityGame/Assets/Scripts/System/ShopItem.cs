using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private ShipUpgradeConfigScriptableObject config;
    public ShipUpgradeConfigScriptableObject Config { get { return config; } }
    private bool isLocked = true;
    public bool IsLocked { get { return isLocked; } }

    public Sprite Icon { get { return config.Icon; } }
    public string Name { get { return config.Name; } }
    public string Description { get { return config.Description; } }
    public ShipUpgradeType UpgradeType { get { return config.UpgradeType; } }
    public int UpgradeTier { get { return config.UpgradeTier; } }
    public ShopCost Cost { get { return config.Cost; } }

    private bool isBought = false;
    public bool IsBought { get { return isBought; } }

    public void Initialize(ShipUpgradeConfigScriptableObject config)
    {
        this.config = config;
        name = $"ShopItem - {config.UpgradeType} - {config.UpgradeTier}";
    }

    public void Buy() {
        isBought = true;
    }

    public void Unlock()
    {
        isLocked = false;
        UIManager.main.ShowShopItem(this);
    }

}
