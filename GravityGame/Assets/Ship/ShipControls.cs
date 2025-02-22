using System;
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
    private float yInput = 0.0f;

    private Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        forward = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, camera.transform.rotation, rotateSpeed * Time.deltaTime);

        var t = 1.0f;
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Mouse Y");
        var z = Input.GetAxis("Mouse X");
        yInput += y;
        yInput += -Math.Sign(yInput) * 10f * Time.deltaTime;
        //if(!WorldManager.main.invertControls) y = -y;

        var horizontalRotateSpeed = Mathf.Lerp(minHorizontalRotateSpeed, maxHorizontalRotateSpeed, t);
        var verticalRotateSpeed = Mathf.Lerp(minVerticalRotateSpeed, maxVerticalRotateSpeed, t);
        var yawRotation = shipMesh.transform.right.y * Mathf.Lerp(10.0f, 25.0f, (1-t));

        var roll = z * Time.deltaTime * verticalRotateSpeed;
        var pitch = 0.01f * yInput * Time.deltaTime * verticalRotateSpeed;
        var yaw = -x * Time.deltaTime * horizontalRotateSpeed;

        forward = forward * Quaternion.AngleAxis(roll, transform.forward);
        forward = forward * Quaternion.AngleAxis(pitch, transform.right);
        forward = forward * Quaternion.AngleAxis(yaw, transform.up);

        shipMesh.transform.Rotate(-Vector3.forward, roll);
        shipMesh.transform.Rotate(Vector3.right, pitch);
        shipMesh.transform.Rotate(-Vector3.up, yaw);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = shipMesh.transform.forward * 10.0f;
    }
}
