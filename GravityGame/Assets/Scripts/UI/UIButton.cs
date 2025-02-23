
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{

    [SerializeField]
    private UnityEvent Action;
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI txtCanBuy;

    public void Disable() {
        button.enabled = false;
    }

    public void PerformAction() {
        Action.Invoke();
    }

    public void CanBuy(bool canBuy) {
        if (canBuy) {
            txtCanBuy.enabled = false;
        } else {
            txtCanBuy.enabled = true;
        }
    }

}
