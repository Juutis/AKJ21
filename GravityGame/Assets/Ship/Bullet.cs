using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private ParticleSystem poof;

    public void Init(Vector3 position, Vector3 velocity) {
        rb = GetComponent<Rigidbody>();
        transform.position = position;
        rb.position = position;
        rb.linearVelocity = velocity;
        Invoke("Kill", 5.0f);
    }

    public void Kill() {
        poof.Play();
        poof.transform.parent = null;
        Destroy(gameObject);
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
