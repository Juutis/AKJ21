using UnityEngine;

public class GobboShip : MonoBehaviour
{
    [SerializeField]
    private Cannon cannon;

    private float shootInterval = 0.1f;
    private float shootTimer = 0.0f;

    private GameObject player;

    private float maxShootDistance = 100.0f;
    private float maxShootAngle = 30.0f;
    private float rotateSpeed = 1.0f;

    private Rigidbody rb;

    private float moveSpeed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
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
        rb.linearVelocity = gobboToPlayer.normalized * moveSpeed;
    }

    private void rotateTowardsPlayer() {
        var gobboToPlayer = player.transform.position - transform.position;
        transform.forward = Vector3.RotateTowards(transform.forward, gobboToPlayer, rotateSpeed * Time.deltaTime, 0.0f);
    }

    private void handleShooting() {
        var gobboToPlayer = player.transform.position - transform.position;
        var angle = Vector3.Angle(transform.forward, gobboToPlayer);
        if (gobboToPlayer.magnitude < maxShootDistance && angle < maxShootAngle) {
            shoot();
        }
    }
    
    private void shoot() {
        if (shootTimer < Time.time) {
            cannon.Fire(0.1f);
            shootTimer = Time.time + shootInterval;
        }
    }
}
