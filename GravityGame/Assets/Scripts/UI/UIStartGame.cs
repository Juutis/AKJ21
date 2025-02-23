using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIStartGame : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool isShown = true;
    [SerializeField]
    private Image imgCursor;

    [SerializeField]
    private GameObject container;

    [SerializeField]
    private GameObject button;
    [SerializeField]
    private RectTransform scrollRt;

    [SerializeField]
    private Vector2 scrollTarget;
    private Vector2 originalScroll;
    private bool isStarted = false;

    void Start()
    {
        originalScroll = scrollRt.anchoredPosition;


        Debug.Log("Version Final 1.5.1");

if ( SceneManager.GetActiveScene().name == "Level1") {
        MusicPlayer.main.PlayMusic(MusicType.MainMenu);
        Time.timeScale = 0f;
        container.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
} else {
        MusicPlayer.main.PlayMusic(MusicType.Game);
        Time.timeScale = 1f;
        container.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        imgCursor.enabled = false;
    }
        button.SetActive(false);
        LevelGenerator.main.NextLevel();

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
            MusicPlayer.main.PlayMusic(MusicType.Game);
        });
    }

    float scrollSpeed = 20f;
    float scrollSpeedWithAnyKey = 100f;

    bool scrollFinished = false;

    void Update()
    {
        if (!isStarted) {
            if (Input.anyKeyDown) {
                isStarted = true;
            }
            return;
        }
        if (!isShown)
        {
            return;
        }

        if (!scrollFinished) {
            Vector2 deltaScroll = scrollRt.anchoredPosition;
            if (Input.anyKey) {
                deltaScroll.y += scrollSpeedWithAnyKey * Time.unscaledDeltaTime;
            } else {
                deltaScroll.y += scrollSpeed * Time.unscaledDeltaTime;
            }
            scrollRt.anchoredPosition = deltaScroll;
            if (scrollRt.anchoredPosition.y >= scrollTarget.y) {
                scrollFinished = true;
                button.SetActive(true);
            }
        }

        if(container.activeSelf) {
            Cursor.lockState = CursorLockMode.None;
            imgCursor.transform.position = Input.mousePosition;
        }
    }
}
