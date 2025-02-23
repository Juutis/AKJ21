using UnityEngine;

public class UIEndGame : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool isShown = false;

    [SerializeField]
    private GameObject container;

    void Start()
    {
        container.SetActive(false);
    }

    public void Show() {
        if (isShown) {
            return;
        }
        isShown = true;
        Time.timeScale = 0f;
        UIManager.main.ShowCurtains(delegate {
            container.SetActive(true);
            UIManager.main.HideCurtains(delegate{
                animator.Play("theEndShow");
            });
        });
    }

    void Update()
    {
        if (!isShown)
        {
            return;
        }
        Cursor.lockState = CursorLockMode.None;
    }
}
