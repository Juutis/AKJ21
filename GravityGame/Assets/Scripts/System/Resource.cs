using UnityEngine;

public class Resource: MonoBehaviour
{
    [SerializeField]
    private ResourceConfigScriptableObject config;
    public int Weight {get { return config.Weight; }}
    public ResourceType Type {get { return config.ResourceType; }}
}


