using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ShipHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;

    [SerializeField]
    private float shieldRechargeAmount;
    [SerializeField]
    private float shieldRechargeCD;

    [SerializeField]
    private float hitCD = 1f;

    private float lastHit = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Hit()
    {
        if (Time.time - lastHit < hitCD)
        {
            return;
        }

        lastHit = Time.time;

        currentHP--;

        //foreach (Material material in materials)
        //{
        //    material.color = Color.Lerp(matColorCache[material], Color.red, ((float)maxHP - currentHP) / maxHP * 0.666f);
        //}

        if (currentHP <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Debug.Log("GAME OVER YOU LOST LOSER LOL");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Hit();
        }
    }
}
