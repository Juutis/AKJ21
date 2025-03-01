using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GobboShip : MonoBehaviour
{
    [SerializeField]
    private Cannon cannon;

    [SerializeField]
    private int maxHP;
    private int currentHP;

    [SerializeField]
    private List<DropRate> dropRates = new();

    private List<Rigidbody> drops = new();

    private float shootInterval = 0.5f;
    private float shootTimer = 0.0f;

    private GameObject player;

    private float maxShootDistance = 80.0f;
    private float maxShootAngle = 30.0f;
    private float rotateSpeed = 90.0f;

    private Rigidbody rb;

    private float moveSpeed = 5.0f;
    private Transform alignTransform;

    private List<Material> materials = new();
    private Dictionary<Material, Color> matColorCache = new();
    private float lastHit = 0f;
    private float hitCD = 0f;

    [SerializeField]
    private GameObject dieExplosionPrefab;

    private ShipControls shipControls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shipControls = player.GetComponent<ShipControls>();
        alignTransform = player.transform.Find("Ship");
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;

        foreach (Transform wrapper in transform)
        {
            foreach (Transform child in wrapper.transform)
            {
                if (child.TryGetComponent(out MeshRenderer childMesh))
                {
                    materials.AddRange(childMesh.materials);

                    childMesh.materials.ToList().ForEach(x => matColorCache.Add(x, x.color));
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (shipControls.isDead) {
            return;
        }
        rotateTowardsPlayer();
        handleShooting();
    }

    void FixedUpdate()
    {
        var gobboToPlayer = player.transform.position - transform.position;
        var distance = gobboToPlayer.magnitude;
        var t = distance / 20.0f;
        var ms = Mathf.Lerp(-moveSpeed, moveSpeed, t);
        
        if (distance > 100.0f || shipControls.isDead) {
            ms = 0;
        }

        rb.linearVelocity = gobboToPlayer.normalized * ms;

        var worldOrigin = WorldOrigin.OfActiveWorld;
        var diff = worldOrigin.transform.position - transform.position;
        var dist = diff.magnitude;
        if (dist < 50) {
            var force = (50.0f - dist) / 50.0f;
            force = Mathf.Clamp01(force);
            rb.AddForce(-diff.normalized * force * 100.0f, ForceMode.Acceleration);
        }
    }

    private void rotateTowardsPlayer() {
        var gobboToPlayer = player.transform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(gobboToPlayer, alignTransform.transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void handleShooting() {
        var gobboToPlayer = player.transform.position - transform.position;
        var angle = Vector3.Angle(transform.forward, gobboToPlayer);
        if (gobboToPlayer.magnitude < maxShootDistance && angle < maxShootAngle) {
            shoot();
        }
    }

    private void Hit()
    {
        if (Time.time - lastHit < hitCD)
        {
            return;
        }

        lastHit = Time.time;

        currentHP--;

        foreach (Material material in materials)
        {
            material.color = Color.Lerp(matColorCache[material], Color.red, ((float)maxHP - currentHP) / maxHP * 0.666f);
        }

        if (currentHP <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        var explosion = Instantiate(dieExplosionPrefab);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
        foreach (DropRate dropRate in dropRates)
        {
            if (dropRate.Chance > Random.value)
            {
                GameObject drop = Instantiate(dropRate.Prefab);
                drops.Add(drop.GetComponent<Rigidbody>());
                drop.transform.position = transform.position + Random.onUnitSphere * 0.5f;
                drop.SetActive(false);
            }
        }
        foreach (Rigidbody drop in drops)
        {
            Invoke("DisableDropBodies", 1f);
            drop.gameObject.SetActive(true);
            drop.transform.position = transform.position + Random.onUnitSphere;
            drop.linearVelocity = (drop.transform.position - transform.position).normalized * (Random.value * 4f + 3f);
        }
    }

    private void shoot() {
        if (shootTimer < Time.time) {
            cannon.Fire(0.1f);
            shootTimer = Time.time + shootInterval;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            Hit();
            var bullet = other.GetComponent<Bullet>();
            bullet.Kill();
        }
    }
}
