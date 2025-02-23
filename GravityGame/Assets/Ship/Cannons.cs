using System;
using System.Collections.Generic;
using UnityEngine;

public class Cannons : MonoBehaviour
{
    [SerializeField]
    private List<CannonGroup> tier0CannonGroups;

    [SerializeField]
    private List<CannonGroup> tier1CannonGroups;

    [SerializeField]
    private List<CannonGroup> tier2CannonGroups;

    private float shootTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)) {
            Shoot();
        }
    }

    public void Shoot() {
        if (shootTimer < Time.time) {
            SoundManager.main.PlaySound(GameSoundType.Cannon);
            var shootInterval = FireNextCannonGroup();
            shootTimer = Time.time + shootInterval;
        }
    }

    private int currentCannonGroup = 0;

    private float FireNextCannonGroup() {
        var cannonLevel = ShipUpgradeManager.main.GetCurrentHighestUpgrade(ShipUpgradeType.Cannon);
        var cannonGroups = tier0CannonGroups;
        var shootInterval = 0.2f;
        if (cannonLevel.IntValue == 1) {
            cannonGroups = tier1CannonGroups;
            shootInterval = 0.125f;
        } else if (cannonLevel.IntValue == 2) {
            cannonGroups = tier2CannonGroups;
            shootInterval = 0.05f;
        }
        var cannonGroup = cannonGroups[currentCannonGroup];
        foreach(var c in cannonGroup.cannons) {
            c.Fire();
        }
        currentCannonGroup++;
        if (currentCannonGroup >= cannonGroups.Count) {
            currentCannonGroup = 0;
        }
        return shootInterval;
    }
}

[Serializable]
public class CannonGroup {
    public List<Cannon> cannons;
}
