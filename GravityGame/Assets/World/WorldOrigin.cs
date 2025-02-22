using UnityEngine;

public class WorldOrigin : MonoBehaviour
{
    [SerializeField]
    public float WorldRadius;

    public static WorldOrigin OfActiveWorld;

    [SerializeField]
    private ParticleSystem stardust;
    private ParticleSystemRenderer stardustRenderer;

    [SerializeField]
    private Color minColor;

    [SerializeField]
    private Color maxColor;


    void Awake()
    {
        OfActiveWorld = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stardustRenderer = stardust.GetComponent<ParticleSystemRenderer>();
        var shape = stardust.shape;
        shape.radius = WorldRadius;
        SetPlayerDistance(0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetPlayerDistance(float dist)
    {
        var t = dist / WorldRadius;
        t = Mathf.Clamp01(t);
        stardustRenderer.sharedMaterial.color = Color.Lerp(minColor, maxColor, t);
    }
}
