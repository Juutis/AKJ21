using UnityEngine;

public class RotateRandomly : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var rotation = new Vector3(Random.Range(0.0f, 360f), Random.Range(0.0f, 360f), Random.Range(0.0f, 360f));
        transform.Rotate(rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
