using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSystem : MonoBehaviour
{
    void Awake()
    {

        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameSystem");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
            Debug.Log("Destroying the new one I hope");
        }
        DontDestroyOnLoad(this);
        gameObject.name = "System32";
    }


    void Start()
    {
        Debug.Log("There should be just one of me");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            SceneManager.LoadScene("Level2");
        }
    }
}
