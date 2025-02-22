using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class GobboShip : MonoBehaviour
{
    [SerializeField]
    private Cannon cannon;

    [SerializeField]
    private int maxHP;
    private int currentHP;

    private float shootInterval = 0.5f;
    private float shootTimer = 0.0f;

    private GameObject player;

    private float maxShootDistance = 100.0f;
    private float maxShootAngle = 30.0f;
    private float rotateSpeed = 90.0f;

    private Rigidbody rb;

    private float moveSpeed = 5.0f;
    private Transform alignTransform;

    private List<Material> materials = new();
    private Dictionary<Material, Color> matColorCache = new();
    private float lastHit = 0f;
    private float hitCD = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        rotateTowardsPlayer();
        handleShooting();
    }

    void FixedUpdate()
    {
        var gobboToPlayer = player.transform.position - transform.position;
        var distance = gobboToPlayer.magnitude;
        var t = distance / 20.0f;
        var ms = Mathf.Lerp(-moveSpeed, moveSpeed, t);
        rb.linearVelocity = gobboToPlayer.normalized * ms;
    }

    private void rotateTowardsPlayer() {
        var gobboToPlayer = player.transform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(gobboToPlayer, player.transform.up);
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
        Destroy(gameObject);
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
            Destroy(other.gameObject);
        }
    }
}
