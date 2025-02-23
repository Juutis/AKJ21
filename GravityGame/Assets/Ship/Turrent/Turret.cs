using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Turret : MonoBehaviour
{
    private GameObject player;
    private Transform alignTransform;
    private float rotateSpeed = 60.0f;

    [SerializeField]
    private int maxHP;
    private int currentHP;

    [SerializeField]
    private List<DropRate> dropRates = new();

    private List<Rigidbody> drops = new();
    
    private List<Material> materials = new();
    private Dictionary<Material, Color> matColorCache = new();
    private float lastHit = 0f;
    private float hitCD = 0f;

    
    [SerializeField]
    private GameObject dieExplosionPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        alignTransform = player.transform.Find("Ship");

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
    }

    // Update is called once per frame
    void Update()
    {  
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

        foreach (Rigidbody drop in drops)
        {
            Invoke("DisableDropBodies", 1f);
            drop.gameObject.SetActive(true);
            drop.transform.position = transform.position + Random.onUnitSphere;
            drop.linearVelocity = (drop.transform.position - transform.position).normalized * (Random.value * 4f + 3f);
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
