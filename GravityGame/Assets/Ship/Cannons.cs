using System;
using System.Collections.Generic;
using UnityEngine;

public class Cannons : MonoBehaviour
{
    [SerializeField]
    private List<CannonGroup> cannonGroups;

    private float shootInterval = 0.1f;
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
            FireNextCannonGroup();
            shootTimer = Time.time + shootInterval;
        }
    }

    private int currentCannonGroup = 0;

    private void FireNextCannonGroup() {
        var cannonGroup = cannonGroups[currentCannonGroup];
        foreach(var c in cannonGroup.cannons) {
            c.Fire();
        }
        currentCannonGroup++;
        if (currentCannonGroup >= cannonGroups.Count) {
            currentCannonGroup = 0;
        }
    }
}

[Serializable]
public class CannonGroup {
    public List<Cannon> cannons;
}
