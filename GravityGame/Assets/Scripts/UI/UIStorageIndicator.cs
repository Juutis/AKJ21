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
    }

    public void UpdateView()
    {
        if (!container.gameObject.activeSelf)
        {
            container.gameObject.SetActive(true);
        }
        float fillAmount = inventory.CurrentWeight / (float)inventory.GetMaxStorage();
        imgProgress.fillAmount = fillAmount;
        txtAmount.text = $"{inventory.CurrentWeight} / {inventory.GetMaxStorage()}";
    }
}
