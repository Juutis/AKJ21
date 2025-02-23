using UnityEngine;
using UnityEngine.UI;

public class UIShipHealth : MonoBehaviour
{
    private ShipHealth shipHealth;

    [SerializeField]
    private Image imgFill;
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            shipHealth = player.GetComponent<ShipHealth>();
        }
        UpdateHealth();
    }

    public void UpdateHealth() {
        if (shipHealth == null) {
            return;
        }
        imgFill.fillAmount = shipHealth.CurrentHp / (1.0f*shipHealth.MaxHp);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }
}
