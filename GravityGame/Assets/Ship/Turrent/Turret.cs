using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameObject player;
    private Transform alignTransform;
    private float rotateSpeed = 60.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        alignTransform = player.transform.Find("Ship");
    }

    // Update is called once per frame
    void Update()
    {
        //rotateTowardsPlayer();   
    }

    private void rotateTowardsPlayer() {
        var gobboToPlayer = player.transform.position - transform.position;
        var targetRotation = Quaternion.LookRotation(transform.forward, player.transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
