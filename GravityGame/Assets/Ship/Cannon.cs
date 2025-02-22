using Unity.Cinemachine.Samples;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private float shootInterval = 0.1f;
    private float shootTimer = 0.0f;

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
        if (Input.GetKey(KeyCode.Mouse0)) {
            Shoot();
        }
        
    }

    public void Shoot() {
        if (shootTimer < Time.time) {
            Fire();
            shootTimer = Time.time + shootInterval;
        }
    }

    private void Fire() {
        var bullet = Instantiate(bulletPrefab);
        bullet.Init(muzzle.position, transform.forward * 100.0f);
    }
}
