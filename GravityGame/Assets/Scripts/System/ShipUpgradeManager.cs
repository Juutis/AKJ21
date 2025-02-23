using System.Collections.Generic;
using UnityEngine;

public class ShipUpgradeManager : MonoBehaviour
{
    //private List<ShipUpgradeType> upgradeTypes;
    //private List<ShipUpgradeConfigScriptableObject> initialUpgrades;

    public static ShipUpgradeManager main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private List<ShipUpgradeConfigScriptableObject> initialUpgrades;

    void Start()
    {
        Debug.Log($"Applying {initialUpgrades.Count} initial upgrades");
        foreach(ShipUpgradeConfigScriptableObject upgradeConfig in initialUpgrades) {
            var upgrade = Instantiate(shipUpgradePrefab, elementContainer);
            upgrade.Initialize(upgradeConfig);
            upgrade.Apply();
            upgrades.Add(upgrade);
        }
    }

    [SerializeField]
    private ShipUpgrade shipUpgradePrefab;
    [SerializeField]
    private Transform elementContainer;
    private List<ShipUpgrade> upgrades = new ();

    public void ApplyUpgrade(ShopItem shopItem)
    {
        var upgrade = Instantiate(shipUpgradePrefab, elementContainer);
        upgrade.Initialize(shopItem.Config);
        upgrades.Add(upgrade);
        upgrade.Apply();
    }

    public ShipUpgrade GetCurrentHighestUpgrade(ShipUpgradeType upgradeType ) {
        //var highestUpgrade = upgrades.FindLast(u => u.UpgradeType == upgradeType);
        ShipUpgrade highestUpgrade = null;
        foreach(ShipUpgrade upgrade in upgrades) {
            Debug.Log($"{upgrade.UpgradeType} {upgrade.UpgradeTier}");
            if (upgrade.UpgradeType == upgradeType) {
                if (highestUpgrade == null || highestUpgrade.UpgradeTier < upgrade.UpgradeTier) {
                    highestUpgrade = upgrade;
                }
            }
        }
        if (highestUpgrade != null) {
            Debug.Log($"Highest upgrade for {upgradeType} is {highestUpgrade.UpgradeTier}");
        } else {
            Debug.Log($"No upgrades for {upgradeType}");
        }
        return highestUpgrade;
    }
} 
