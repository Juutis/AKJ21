using UnityEngine;

public class Resource: MonoBehaviour
{
    [SerializeField]
    private ResourceConfigScriptableObject config;
    public int Weight {get { return config.Weight; }}
    public Sprite Icon {get { return config.Icon; }}
    public Color Color {get { return config.Color; }}
    public string Name {get { return config.Name; }}
    public string Description {get { return config.Description; }}

    public ResourceType ResourceType {get { return config.ResourceType; }}

    public void Initialize(ResourceConfigScriptableObject config)
    {
        this.config = config;
        name = $"Resource - {config.ResourceType}";
    }

}


