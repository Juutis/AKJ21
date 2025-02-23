using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class DestroyablePortal : MonoBehaviour
{
    [SerializeField]
    private int laserLevelRequirement = 0;

    [SerializeField]
    private GameObject portalTrigger;

    [SerializeField]
    private GameObject portalShield;

    [SerializeField]
    private int maxHP;
    private int currentHP;

    private List<Material> materials = new();
    private Dictionary<Material, Color> matColorCache = new();
    private float lastHit = 0f;
    private float hitCD = .2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;

        foreach (Transform wrapper in transform)
        {
            string tag = wrapper.tag;

            if (tag == "portalshield" || tag == "portalfield")
            {
                continue;
            }

            if (wrapper.TryGetComponent(out MeshRenderer wrapperMesh))
            {
                materials.AddRange(wrapperMesh.materials);

                wrapperMesh.materials.ToList().ForEach(x => matColorCache.Add(x, x.color));
            }

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
            material.color = Color.Lerp(matColorCache[material], Color.red, ((float)maxHP - currentHP) / maxHP * 0.666f);
        }

        if (currentHP <= 0)
        {
            Explode();
        }
    }
    private void Explode()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;

        portalTrigger.SetActive(true);
        portalShield.SetActive(false);
    }

}
