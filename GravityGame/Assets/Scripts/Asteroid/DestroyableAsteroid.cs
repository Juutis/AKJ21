using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DestroyableAsteroid : MonoBehaviour
{
    [SerializeField]
    private int maxHP;
    private int currentHP;
    [SerializeField]
    private List<DropRate> dropRates = new();

    private List<Rigidbody> drops = new();

    private List<GameObject> pieces = new();
    private List<Material> materials = new();
    private Dictionary<Material, Color> matColorCache = new();
    private float lastHit = 0f;
    private float hitCD = .2f;

    [SerializeField]
    private GameObject explosionPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;

        foreach (Transform wrapper in transform)
        {
            foreach (Transform child in wrapper.transform)
            {
                if (child.TryGetComponent(out MeshRenderer childMesh))
                {
                    pieces.Add(childMesh.gameObject);
                    materials.AddRange(childMesh.materials);

                    childMesh.materials.ToList().ForEach(x => matColorCache.Add(x, x.color));
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hit()
    {
        if (Time.time - lastHit < hitCD)
        {
            return;
        }

        lastHit = Time.time;

        currentHP--;

        foreach (Material material in materials)
        {
            material.color = Color.Lerp(matColorCache[material], Color.red, ((float)maxHP - currentHP)/maxHP * 0.666f);
        }

        if (currentHP <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;

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

        //Invoke("Delete", 5f);

        foreach (Rigidbody drop in drops)
        {
            Invoke("DisableDropBodies", 1f);
            drop.gameObject.SetActive(true);
            drop.linearVelocity = (drop.transform.position - transform.position).normalized * (Random.value * 5f + 5f);
        }
        var expl = Instantiate(explosionPrefab);
        expl.transform.position = transform.position;
        Delete();
    }

    private void Delete()
    {
        foreach (GameObject piece in pieces)
        {
            Destroy(piece);
        }

        Destroy(gameObject);
    }

    private void DisableDropBodies()
    {
        foreach (Rigidbody drop in drops)
        {
            drop.isKinematic = false;
        }
    }
}

[Serializable]
public class DropRate {
    public float Chance;
    public GameObject Prefab;
}