using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipControls : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private GameObject shipMesh;
    
    private Quaternion forward;

    private float rotateSpeed = 360.0f;

    private float minHorizontalRotateSpeed = 180.0f;
    private float maxHorizontalRotateSpeed = 180.0f;
    private float minVerticalRotateSpeed = 180.0f;
    private float maxVerticalRotateSpeed = 180.0f;
    private float xInput = 0.0f;
    private float yInput = 0.0f;

    private Rigidbody rb;

    [SerializeField]
    private CinemachineThirdPersonFollow thirdPersonCamera;

    private float speed;
    private float maxSpeed = 20.0f;
    private float minSpeed = -5.0f;

    [SerializeField]
    private ParticleSystem booster;

    
    [SerializeField]
    private ParticleSystem laser;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        forward = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        zoom = thirdPersonCamera.CameraDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            if (!laser.isPlaying) {
                laser.Play();
            }
        } else {
            laser.Stop();
        }

        handleZoom();
        handleAcceleration();

        var t = 1.0f;
        var x = Input.GetAxisRaw("Mouse X");
        var y = Input.GetAxisRaw("Mouse Y");
        var z = Input.GetAxis("Horizontal");
        //if(!WorldManager.main.invertControls) y = -y;
        
        xInput += x;
        yInput += y;

        xInput = Mathf.Clamp(xInput, -200.0f, 200.0f);
        yInput = Mathf.Clamp(yInput, -200.0f, 200.0f);

        var horizontalRotateSpeed = Mathf.Lerp(minHorizontalRotateSpeed, maxHorizontalRotateSpeed, t);
        var verticalRotateSpeed = Mathf.Lerp(minVerticalRotateSpeed, maxVerticalRotateSpeed, t);

        var roll = z * Time.deltaTime * verticalRotateSpeed;
        var pitch = -0.01f * yInput * Time.deltaTime * verticalRotateSpeed;
        var yaw = -0.01f * xInput * Time.deltaTime * horizontalRotateSpeed;

        forward = forward * Quaternion.AngleAxis(roll, transform.forward);
        forward = forward * Quaternion.AngleAxis(pitch, transform.right);
        forward = forward * Quaternion.AngleAxis(yaw, transform.up);

        shipMesh.transform.Rotate(-Vector3.forward, roll);
        shipMesh.transform.Rotate(Vector3.right, pitch);
        shipMesh.transform.Rotate(-Vector3.up, yaw);
        
        //xInput = Mathf.MoveTowards(xInput, 0.0f, 40f * Time.deltaTime);
        //yInput = Mathf.MoveTowards(yInput, 0.0f, 20f * Time.deltaTime);
        xInput = xInput * Mathf.Pow(0.9f, Time.deltaTime*30);
        yInput = yInput * Mathf.Pow(0.9f, Time.deltaTime*30);
    }

    void FixedUpdate()
    {
        var targetSpeed = shipMesh.transform.forward * speed;
        rb.linearVelocity = Vector3.RotateTowards(rb.linearVelocity, targetSpeed, 2.0f * Time.deltaTime, 2.0f * Time.deltaTime);

        var worldOrigin = WorldOrigin.OfActiveWorld;
        var diff = worldOrigin.transform.position - transform.position;
        var dist = diff.magnitude;
        if (dist > worldOrigin.WorldRadius) {
            var t = (dist - worldOrigin.WorldRadius) / 100.0f;
            t = Mathf.Clamp01(t);
            rb.AddForce(diff.normalized * t * 1000.0f, ForceMode.Acceleration);
        }
        worldOrigin.SetPlayerDistance(dist);
    }

    private float minZoom = 2.0f;
    private float maxZoom = 15.0f;
    private float zoom;
    private float zoomDiff;

    void handleZoom() {
        zoomDiff -= Input.GetAxis("Mouse ScrollWheel") * 5;
        zoom += zoomDiff * Time.deltaTime;
        if (zoom > maxZoom) {
            zoom = maxZoom;
            zoomDiff = 0.0f;
        }
        if (zoom < minZoom) {
            zoom = minZoom;
            zoomDiff = 0.0f;
        }
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        thirdPersonCamera.CameraDistance = zoom;
        zoomDiff = Mathf.MoveTowards(zoomDiff, 0.0f, 10f * Time.deltaTime);
    }

    void handleAcceleration() {
        var input = Input.GetAxis("Vertical");
        speed += input * Time.deltaTime * 30.0f;
        //speed = Mathf.MoveTowards(speed, 0.0f, 5.0f * Time.deltaTime);
        speed = speed * Mathf.Pow(0.9f, Time.deltaTime*30);
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        if (input > 0.0f) {
            booster.Play();
        } else {
            booster.Stop();
        }
    }
}
