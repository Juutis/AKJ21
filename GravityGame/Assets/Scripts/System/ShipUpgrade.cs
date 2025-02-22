using UnityEngine;

public class ShipUpgrade : MonoBehaviour
{

    private ShipUpgradeConfigScriptableObject config;

    private bool isApplied = false;
    public bool IsApplied { get { return isApplied; } }

    public ShipUpgradeType UpgradeType { get { return config.UpgradeType; } }
    public int UpgradeTier { get { return config.UpgradeTier; } }

    public void Initialize(ShipUpgradeConfigScriptableObject upgradeConfig)
    {
        config = upgradeConfig;
        name = $"ShipUpgrade - {config.UpgradeType} - {config.UpgradeTier}";
    }

    public void Apply() {
        if (isApplied) {
            Debug.Log("ALREADY APPLIED: " + config.Name);
            return;
        }
        Debug.Log("Applying ShipUpgrade: {config.UpgradeType} - {config.UpgradeTier}");
        if (config.TypeConfig.UpgradeType == ShipUpgradeType.Cannon) {
            // implement cannon upgrade
        } else if (config.TypeConfig.UpgradeType == ShipUpgradeType.Storage) {
            // add any visual changes here
            // actual Storage is implemented in Inventory.cs using ShipUpgradeManager
        } else if (config.TypeConfig.UpgradeType == ShipUpgradeType.Shield) {
            // implement shield upgrade
        } else if (config.TypeConfig.UpgradeType == ShipUpgradeType.Laser) {
            // implement laser upgrade
        } else if (config.TypeConfig.UpgradeType == ShipUpgradeType.Engine) {
            // implement engine upgrade
        } else if (config.TypeConfig.UpgradeType == ShipUpgradeType.Harvester) {
            // implement harvester upgrade
        }
        isApplied = true;
    }

}


