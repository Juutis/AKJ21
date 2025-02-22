using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer mesh;

    private float maxThickness = 0.685f;
    private float minThickness = 0.92f;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.up = -mainCamera.transform.forward;
        float thickness = 1 / (Vector3.Distance(transform.position, mainCamera.transform.position) / 40f);

        mesh.sharedMaterial.SetFloat("_Cutoff", Mathf.Clamp(thickness, maxThickness, minThickness));
    }
}
