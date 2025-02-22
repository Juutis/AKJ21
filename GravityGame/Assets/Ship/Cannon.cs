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

    
    [SerializeField]
    private float bulletSpeed = 100.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    public void Fire(float variance = 0) {
        var bullet = Instantiate(bulletPrefab);
        var direction = transform.forward + Random.Range(-variance, variance) * transform.right + Random.Range(-variance, variance) * transform.up;
        bullet.Init(muzzle.position, direction.normalized * bulletSpeed);
    }
}
