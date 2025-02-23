using System.Collections.Generic;
using UnityEngine;

public class GobboSpawnArea : MonoBehaviour
{

    [SerializeField]
    private float triggerRadius = 30.0f;

    [SerializeField]
    private float minSpawnRadius = 0.0f;

    [SerializeField]
    private float maxSpawnRadius = 10.0f;

    [SerializeField]
    private float resetDistance = 200.0f;

    [SerializeField]
    private List<GameObject> stuffToSpawn;

    private GameObject player;

    private bool ready = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(player.transform.position, transform.position);
        if (ready && distance < triggerRadius) {
            spawn();
            ready = false;
        }

        if (!ready && distance > resetDistance) {
            //ready = true;
        }
    }

    private void spawn() {
        foreach(var prefab in stuffToSpawn) {
            var offset = Random.onUnitSphere * Random.Range(minSpawnRadius, maxSpawnRadius);
            var gobbo = Instantiate(prefab, transform.parent);
            gobbo.transform.position = transform.position + offset;
        }
    }
}
