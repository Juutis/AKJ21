using System.Collections.Generic;
using UnityEngine;

public class ShipUpgradeManager : MonoBehaviour
{
    //private List<ShipUpgradeType> upgradeTypes;
    //private List<ShipUpgradeConfigScriptableObject> initialUpgrades;

    [SerializeField]
    private ShipUpgrade shipUpgradePrefab;
    [SerializeField]
    private Transform elementContainer;
    private List<ShipUpgrade> upgrades = new ();

    public void ApplyUpgrade(ShopItem shopItem)
    {
        var upgrade = Instantiate(shipUpgradePrefab, elementContainer);
        upgrade.Initialize(shopItem.Config);
        upgrade.Apply();
        upgrades.Add(upgrade);
    }

    public ShipUpgrade GetCurrentHighestUpgrade(ShipUpgradeType upgradeType ) {
        var highestUpgrade = upgrades.FindLast(u => u.UpgradeType == upgradeType);
        if (highestUpgrade != null) {
            Debug.Log($"Highest upgrade for {upgradeType} is {highestUpgrade.UpgradeTier}");
        } else {
            Debug.Log($"No upgrades for {upgradeType}");
        }
        return highestUpgrade;
    }
} 
