using UnityEngine;

public class UIManager: MonoBehaviour {
    public static UIManager main;
    void Awake()
    {
        main = this;
    } 


    public void ShowMessage(string message)
    {
        Debug.Log(message);
    }
}