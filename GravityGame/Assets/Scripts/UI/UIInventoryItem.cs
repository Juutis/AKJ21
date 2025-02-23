using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIInventoryItem : MonoBehaviour
{
    private InventoryResource resource;
    public ResourceType ResourceType { get { return resource.ResourceType; } }

    public int Amount {get {return resource.Amount;}}

    [SerializeField]
    private Image imgIcon;

    [SerializeField]
    private TextMeshProUGUI txtAmount;

    public void Initialize(InventoryResource resourceNew)
    {
        resource = resourceNew;
        imgIcon.sprite = resource.Icon;
        UpdateAmount();
    }

    public void UpdateAmount()
    {
        txtAmount.text = resource.Amount.ToString();
    }
}
