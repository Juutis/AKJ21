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
        var ttl = 50.0f / velocity.magnitude;
        Invoke("Remove", ttl);
        layerMask = LayerMask.GetMask("Default", "Destroyable");
    }

    public void Kill() {
        poof.Play();
        poof.transform.parent = null;
        Destroy(gameObject);
    }

    public void Remove() {
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        var t = rb.linearVelocity.magnitude * Time.deltaTime;
        var dir = rb.linearVelocity.normalized;
        if (Physics.Raycast(transform.position - dir * t, dir, out hitInfo, t * 3, layerMask)) {
            transform.position = hitInfo.point;
            rb.position = hitInfo.point;
            Kill();
        }
    }

    void FixedUpdate()
    {

    }
}
