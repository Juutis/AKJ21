using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStorageIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform container;

    [SerializeField]
    private Image imgProgress;

    [SerializeField]
    private TextMeshProUGUI txtAmount;

    private Inventory inventory;
    public void Initialize(Inventory newInventory)
    {
        inventory = newInventory;
        container.gameObject.SetActive(true);
    }

    public void UpdateView()
    {
        float fillAmount = inventory.CurrentWeight / (float)inventory.GetMaxStorage();
        imgProgress.fillAmount = fillAmount;
        txtAmount.text = $"{inventory.CurrentWeight} / {inventory.GetMaxStorage()}";
    }
}
