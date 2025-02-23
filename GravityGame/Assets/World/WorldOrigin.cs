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

    private bool isHiding;
    private float hidingStarted = 0f;
    private float hidingStopped = 0f;
    private float hiddenAmount = 0f;
    private float hideDuration = 1f;
    private float cacheDist = 0f;


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
        if (isHiding)
        {
            hiddenAmount = Mathf.Clamp01((Time.time - hidingStarted) / hideDuration);
            stardustRenderer.sharedMaterial.color = Color.Lerp(maxColor, minColor, hiddenAmount);
        }
        else if (hiddenAmount > 0f)
        {
            hiddenAmount = Mathf.Clamp((Time.time - hidingStopped) / hideDuration, 0, cacheDist);
            stardustRenderer.sharedMaterial.color = Color.Lerp(minColor, maxColor, hiddenAmount);
        }
    }

    public void SetPlayerDistance(float dist)
    {
        cacheDist = dist;

        if (isHiding || hiddenAmount > 0)
        {
            return;
        }

        var t = dist / WorldRadius;
        t = Mathf.Clamp01(t);
        stardustRenderer.sharedMaterial.color = Color.Lerp(minColor, maxColor, t);
    }

    public void SetHiding(bool hide)
    {
        if (isHiding == hide)
        {
            return;
        }

        isHiding = hide;

        if (hide)
        {
            Debug.Log("Hiding");
            hidingStarted = Time.time;
        }
        else
        {
            Debug.Log("Unhiding");
            hidingStopped = Time.time;
        }

    }
}
