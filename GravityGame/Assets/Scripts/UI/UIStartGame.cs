using UnityEngine;
using UnityEngine.UI;

public class UIStartGame : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool isShown = true;
    [SerializeField]
    private Image imgCursor;

    [SerializeField]
    private GameObject container;
    void Start()
    {
#if UNITY_EDITOR
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
#else
        Time.timeScale = 0f;
        container.SetActive(true);
#endif
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame() {
        if (!isShown) {
            return;
        }
        isShown = false;
        UIManager.main.ShowCurtains(delegate {
            animator.Play("mainMenuHide");
            imgCursor.enabled = false;
        });

    }

    public void HideFinished() {
        UIManager.main.HideCurtains(delegate {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        });
    }

    void Update()
    {
        if (!isShown)
        {
            return;
        }
        Cursor.lockState = CursorLockMode.None;
        imgCursor.transform.position = Input.mousePosition;
    }
}
