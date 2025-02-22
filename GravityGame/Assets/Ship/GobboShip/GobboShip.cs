using UnityEngine;

public class GobboShip : MonoBehaviour
{
    [SerializeField]
    private Cannon cannon;

    private float shootInterval = 0.5f;
    private float shootTimer = 0.0f;

    private GameObject player;

    private float maxShootDistance = 100.0f;
    private float maxShootAngle = 30.0f;
    private float rotateSpeed = 90.0f;

    private Rigidbody rb;

    private float moveSpeed = 5.0f;
    private Transform alignTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        alignTransform = player.transform.Find("Ship");
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
        var distance = gobboToPlayer.magnitude;
        var t = distance / 20.0f;
        var ms = Mathf.Lerp(-moveSpeed, moveSpeed, t);
        rb.linearVelocity = gobboToPlayer.normalized * ms;
    }

    private void rotateTowardsPlayer() {
        var gobboToPlayer = player.transform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(gobboToPlayer, player.transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
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
