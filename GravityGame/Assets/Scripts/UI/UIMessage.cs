using TMPro;
using UnityEngine;

public class UIMessage : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private TextMeshProUGUI txtMessage;
    public void Initialize(string message)
    {
        txtMessage.text = message;
    }

    public void ShowFinished() {
        Destroy(gameObject);
    }
}
