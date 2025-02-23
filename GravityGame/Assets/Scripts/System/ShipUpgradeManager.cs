using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipUpgradeManager : MonoBehaviour
{
    //private List<ShipUpgradeType> upgradeTypes;
    //private List<ShipUpgradeConfigScriptableObject> initialUpgrades;

    public static ShipUpgradeManager main;
    void Awake()
    {
        if (main == null) {
            main = this;
        }
        Debug.Log("ShipUpgradeManager awake");
    }

    [SerializeField]
    private List<ShipUpgradeConfigScriptableObject> initialUpgrades;

    void OnEnable()
    {
        if (main != this)
        {
            return;
        }
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (main != this) {
            return;
        }
        Debug.Log("I am loaded on scene");
        Debug.Log($"Upgrades: {upgrades.Count}");
        foreach (ShipUpgrade upgrade in upgrades)
        {
            if (upgrade == null)
            {
                Debug.Log("upgrade null");
                continue;
            }
        }
        foreach (ShipUpgradeConfigScriptableObject upgradeConfig in initialUpgrades) {
            var highestUpgrade = GetCurrentHighestUpgrade(upgradeConfig.UpgradeType);
            if (highestUpgrade != null) {
                highestUpgrade.Apply();
                Debug.Log($"Applied highest upgrade: {highestUpgrade.UpgradeType} {highestUpgrade.UpgradeTier}");
            } else {
                var upgrade = Instantiate(shipUpgradePrefab, elementContainer);
                upgrade.Initialize(upgradeConfig);
                upgrades.Add(upgrade);
                upgrade.Apply();
                Debug.Log($"Applied initial upgrade: {upgrade.UpgradeType}");
            }
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
            if (upgrade == null) {
                continue;
            }
            //Debug.Log($"{upgrade.UpgradeType} {upgrade.UpgradeTier}");
            if (upgrade.UpgradeType == upgradeType) {
                if (highestUpgrade == null || highestUpgrade.UpgradeTier < upgrade.UpgradeTier) {
                    highestUpgrade = upgrade;
                }
            }
        }
        if (highestUpgrade != null) {
            //Debug.Log($"Highest upgrade for {upgradeType} is {highestUpgrade.UpgradeTier}");
        } else {
            //Debug.Log($"No upgrades for {upgradeType}");
        }
        return highestUpgrade;
    }
} 
