using System.Collections.Generic;
using Unity.Cinemachine.Samples;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    [SerializeField]
    private Bullet bulletPrefab;

    
    [SerializeField]
    private Transform muzzle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Fire() {
        var bullet = Instantiate(bulletPrefab);
        bullet.Init(muzzle.position, transform.forward * 100.0f);
    }
}
