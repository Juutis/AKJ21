using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private ParticleSystem poof;
    private int layerMask;

    public void Init(Vector3 position, Vector3 velocity) {
        rb = GetComponent<Rigidbody>();
        transform.position = position;
        rb.position = position;
        rb.linearVelocity = velocity;
        Invoke("Kill", 5.0f);
        layerMask = LayerMask.GetMask("Default");
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
        if (Physics.Raycast(transform.position, transform.forward, rb.linearVelocity.magnitude * Time.deltaTime, layerMask)) {
            Kill();
        }
    }
}
