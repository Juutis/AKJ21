using UnityEngine;

[CreateAssetMenu(fileName = "ShipUpgradeType", menuName = "Configs/ShipUpgradeType")]
public class ShipUpgradeTypeConfigScriptableObject : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    public Sprite Icon;

    public ShipUpgradeType UpgradeType;

}
