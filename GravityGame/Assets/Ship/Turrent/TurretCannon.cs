using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TurretCannon : MonoBehaviour
{
    [SerializeField]
    private float maxAngle = 45.0f;
    private GameObject player;
    private Vector3 origForward;
    private float rotationSpeed = 60.0f;

    private float maxShootDistance = 50.0f;
    private float maxShootAngle = 10.0f;
    private float shootTimer;
    private float shootInterval = 1.0f;
    private Cannon cannon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        origForward = transform.forward;
        cannon = GetComponentInChildren<Cannon>();
    }

    // Update is called once per frame
    void Update()
    {
        var dirToPlayer = player.transform.position - transform.position;
        dirToPlayer.Normalize();
        var targetDir = Vector3.RotateTowards(origForward, dirToPlayer, Mathf.Deg2Rad * maxAngle, 0.0f);
        transform.forward = Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed * Time.deltaTime, 0.0f);
        handleShooting();
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
            cannon.Fire(0.05f);
            shootTimer = Time.time + shootInterval;
        }
    }
}
