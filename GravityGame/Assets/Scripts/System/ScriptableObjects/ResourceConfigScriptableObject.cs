using UnityEngine;

[CreateAssetMenu(fileName = "ResourceConfig", menuName = "Configs/Resource", order = 1)]
public class ResourceConfigScriptableObject : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    public Sprite Icon;
    public Color Color;

    public ResourceType ResourceType;

    public int Weight;
}

public enum ResourceType
{
    Hydrogen,
    Titanium,
    Crystal,
    Gobbonium
}