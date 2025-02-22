
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{

    [SerializeField]
    private UnityEvent Action;
    [SerializeField]
    private Button button;

    public void PerformAction() {
        Action.Invoke();
    }

}
