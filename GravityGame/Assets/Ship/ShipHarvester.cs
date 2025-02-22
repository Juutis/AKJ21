using UnityEngine;

public class ShipHarvester : MonoBehaviour
{
    [SerializeField]
    private float harvestRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, harvestRadius, transform.forward, 0.1f, LayerMask.GetMask("Pickup"));

        foreach (RaycastHit hit in hits)
        {
            GameObject obj = hit.transform.gameObject;

            if (obj.TryGetComponent(out PickupResource p))
            {
                ResourceType type = p.ResourceType;
                Debug.Log($"Picked up {type}");
            }
            Destroy(hit.transform.gameObject);
        }
    }
}
