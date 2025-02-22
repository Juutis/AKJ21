using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    public void Init(Vector3 position, Vector3 velocity) {
        rb = GetComponent<Rigidbody>();
        transform.position = position;
        rb.position = position;
        rb.linearVelocity = velocity;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
