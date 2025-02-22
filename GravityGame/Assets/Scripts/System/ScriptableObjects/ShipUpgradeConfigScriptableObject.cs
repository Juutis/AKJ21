using UnityEngine;

[CreateAssetMenu(fileName = "ShipUpgradeConfig", menuName = "Configs/ShipUpgrade", order = -1),]
public class ShipUpgradeConfigScriptableObject : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    public Sprite Icon;

    public ShipUpgradeType UpgradeType;

    [Range(1, 5)]
    public int UpgradeTier = 1;

    public ShopCost Cost;

    public ShopUnlockRequirements unlockRequirements;
}


public enum ShipUpgradeType {
    Guns,
    Storage,
    Shield,
    Laser,
    Engine
}