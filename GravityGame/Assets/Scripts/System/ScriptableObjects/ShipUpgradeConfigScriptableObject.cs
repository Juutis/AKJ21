using UnityEngine;

[CreateAssetMenu(fileName = "ShipUpgradeConfig", menuName = "Configs/ShipUpgrade"),]
public class ShipUpgradeConfigScriptableObject : ScriptableObject
{
    public ShipUpgradeTypeConfigScriptableObject TypeConfig;
    public string Name;
    [TextArea]
    public string Description;
    public Sprite Icon { get { return TypeConfig.Icon; } }

    public ShipUpgradeType UpgradeType { get { return TypeConfig.UpgradeType; } }

    [Range(1, 5)]
    public int UpgradeTier = 1;

    public int IntValue;

    public ShopCost Cost;

    public ShopUnlockRequirements unlockRequirements;
}


public enum ShipUpgradeType {
    Cannon,
    Storage,
    Shield,
    Laser,
    Engine,
    Harvester
}