using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager main;
    [SerializeField]
    private List<ResourceConfigScriptableObject> resourcesConfigs = new();

    [SerializeField]
    private Resource resourcePrefab;
    [SerializeField]
    private Transform elementContainer;

    private List<Resource> resources = new();

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        CreateResources();
    }

    private void CreateResources()
    {
        foreach (ResourceConfigScriptableObject resourceConfig in resourcesConfigs)
        {
            Resource resource = Instantiate(resourcePrefab, elementContainer);
            resource.Initialize(resourceConfig);
            resources.Add(resource);
        }
    }

    public Resource GetResource(ResourceType resourceType)
    {
        return resources.FirstOrDefault(resource => resource.ResourceType == resourceType);
    }
}
