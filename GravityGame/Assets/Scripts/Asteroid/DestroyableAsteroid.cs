using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DestroyableAsteroid : MonoBehaviour
{
    [SerializeField]
    private int HP;
    [SerializeField]
    private List<DropRate> dropRates = new();

    private List<Rigidbody> drops = new();

    private List<GameObject> pieces = new();
    private List<Material> materials = new();
    private float lastHit = 0f;
    private float hitCD = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform wrapper in transform)
        {
            foreach (Transform child in wrapper.transform)
            {
                if (child.TryGetComponent(out MeshRenderer childMesh))
                {
                    pieces.Add(childMesh.gameObject);
                    materials.AddRange(childMesh.materials);
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

    public void Hit()
    {
        if (Time.time - lastHit < hitCD)
        {
            return;
        }

        lastHit = Time.time;

        HP--;

        foreach (Material material in materials)
        {
            StartCoroutine(LerpMat(material, material.color, Color.red));
        }

        if (HP <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;

        foreach (GameObject piece in pieces)
        {
            piece.transform.parent = null;
            Rigidbody body = piece.AddComponent<Rigidbody>();
            body.useGravity = false;
            body.linearVelocity = (piece.transform.position - transform.position).normalized * (Random.value * 15f + 15f);

            Invoke("Delete", 5f);
        }

        foreach (Rigidbody drop in drops)
        {
            Invoke("DisableDropBodies", 1f);
            drop.gameObject.SetActive(true);
            drop.linearVelocity = (drop.transform.position - transform.position).normalized * (Random.value * 5f + 5f);
        }
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

    private IEnumerator LerpMat(Material mat, Color a, Color b)
    {
        for (float i = 0; i < 1; i += 0.01f)
        {
            mat.color = Color.Lerp(a, b, i);
            yield return new WaitForSeconds(0.01f);
        }

        mat.color = a;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Hit();
    }
}

[Serializable]
public class DropRate {
    public float Chance;
    public GameObject Prefab;
}